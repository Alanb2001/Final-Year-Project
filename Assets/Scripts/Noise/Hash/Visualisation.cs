using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

using static Unity.Mathematics.math;

namespace Noise.Hash
{
    public abstract class Visualisation : MonoBehaviour
    {
        private static int positionsId = Shader.PropertyToID("_Positions"),
            normalsId = Shader.PropertyToID("_Normals"),
            configId = Shader.PropertyToID("_Config");

        [SerializeField] private Mesh instanceMesh;
        [SerializeField] private Material material;
        [SerializeField, Range(1, 512)] private int resolution = 16;
        [SerializeField, Range(-0.5f, 0.5f)] private float displacement = 0.1f;
        [SerializeField] private Shape shape;
        [SerializeField, Range(0.1f, 10f)] private float instanceScale = 2f;

        private NativeArray<float3x4> _positions, _normals;

        private ComputeBuffer _positionsBuffer, _normalsBuffer;

        private MaterialPropertyBlock _propertyBlock;

        private bool _isDirty;

        private Bounds _bounds;
        protected abstract void EnableVisualisation(int dataLength, MaterialPropertyBlock propertyBlock);

        protected abstract void DisableVisualisation();

        protected abstract void UpdateVisualisation(NativeArray<float3x4> positions, int resolution, JobHandle handle);

        public enum Shape
        {
            Plane,
            Sphere,
            Torus
        };

        private static Shapes.ScheduleDelegate[] shapeJobs =
        {
            Shapes.Job<Shapes.Plane>.ScheduleParallel, Shapes.Job<Shapes.Sphere>.ScheduleParallel,
            Shapes.Job<Shapes.Torus>.ScheduleParallel,
        };
        
        private void OnEnable()
        {
            _isDirty = true;

            int length = resolution * resolution;
            length = length / 4 + (length & 1);
            _positions = new NativeArray<float3x4>(length, Allocator.Persistent);
            _normals = new NativeArray<float3x4>(length, Allocator.Persistent);
            _positionsBuffer = new ComputeBuffer(length * 4, 3 * 4);
            _normalsBuffer = new ComputeBuffer(length * 4, 3 * 4);

            _propertyBlock ??= new MaterialPropertyBlock();
            EnableVisualisation(length, _propertyBlock);
            _propertyBlock.SetBuffer(positionsId, _positionsBuffer);
            _propertyBlock.SetBuffer(normalsId, _normalsBuffer);
            _propertyBlock.SetVector(configId, new Vector4(resolution, instanceScale / resolution, displacement));
        }

        private void OnDisable()
        {
            _positions.Dispose();
            _normals.Dispose();
            _positionsBuffer.Release();
            _normalsBuffer.Release();
            _positionsBuffer = null;
            _normalsBuffer = null;
            DisableVisualisation();
        }

        private void OnValidate()
        {
            if (_positionsBuffer != null && enabled)
            {
                OnDisable();
                OnEnable();
            }
        }

        private void Update()
        {
            if (_isDirty || transform.hasChanged)
            {
                _isDirty = false;
                transform.hasChanged = false;

                UpdateVisualisation(_positions, resolution,
                    shapeJobs[(int)shape](_positions, _normals, resolution, transform.localToWorldMatrix, default));

                _positionsBuffer.SetData(_positions.Reinterpret<float3>(3 * 4 * 4));
                _normalsBuffer.SetData(_normals.Reinterpret<float3>(3 * 4 * 4));

                _bounds = new Bounds(transform.position, float3(2f * cmax(abs(transform.lossyScale)) + displacement));
            }

            Graphics.DrawMeshInstancedProcedural(instanceMesh, 0, material, _bounds,
                resolution * resolution, _propertyBlock);
        }
    }
}