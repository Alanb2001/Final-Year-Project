using System;
using Unity.Burst;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

using static Unity.Mathematics.math;

namespace Noise.Hash
{
    public class HashVisualisation : MonoBehaviour
    {
        private static int hasesId = Shader.PropertyToID("_Hashes"), configId = Shader.PropertyToID("_Config");

        [SerializeField] private Mesh _instanceMesh;
        [SerializeField] private Material _material;
        [SerializeField, Range(1, 512)] private int _resolution = 16;
        [SerializeField] private int _seed;
        [SerializeField, Range(-2f, 2f)] private float _VerticalOffset = 1f;
        
        private NativeArray<uint> _hashes;

        private ComputeBuffer _hashesBuffer;

        private MaterialPropertyBlock _propertyBlock;
        
        [BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
        struct HashJob : IJobFor
        {
            [WriteOnly] public NativeArray<uint> hashes;

            public int resolution;

            public float invResolution;

            public SmallXXHash hash;
            
            public void Execute(int i)
            {
                int v = (int)floor(invResolution * i + 0.00001f);
                int u = i - resolution * v - resolution / 2;
                v -= resolution / 2;
                
                hashes[i] = hash.Eat(u).Eat(v);
            }
        }

        private void OnEnable()
        {
            int length = _resolution * _resolution;
            _hashes = new NativeArray<uint>(length, Allocator.Persistent);
            _hashesBuffer = new ComputeBuffer(length, 4);

            new HashJob { hashes = _hashes, resolution = _resolution, invResolution = 1f / _resolution, hash = SmallXXHash.Seed(_seed)}
                .ScheduleParallel(_hashes.Length, _resolution, default).Complete();
            
            _hashesBuffer.SetData(_hashes);

            _propertyBlock ??= new MaterialPropertyBlock();
            _propertyBlock.SetBuffer(hasesId, _hashesBuffer);
            _propertyBlock.SetVector(configId, new Vector4(_resolution, 1f / _resolution, _VerticalOffset / _resolution));
        }

        private void OnDisable()
        {
            _hashes.Dispose();
            _hashesBuffer.Release();
            _hashesBuffer = null;
        }

        private void OnValidate()
        {
            if (_hashesBuffer != null && enabled)
            {
                OnDisable();
                OnEnable();
            }
        }

        private void Update()
        {
            Graphics.DrawMeshInstancedProcedural(_instanceMesh, 0, _material, new Bounds(Vector3.zero, Vector3.one),
                _hashes.Length, _propertyBlock);
        }
    }
}
