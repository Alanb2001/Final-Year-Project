using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

using static Unity.Mathematics.math;
using static Noise.Hash.Noise;

namespace Noise.Hash
{
    public class NoiseVisualisation : Visualisation
    {
        private static ScheduleDelegate[,] noiseJobs =
        {
            {
                Job<Lattice1D<Perlin>>.ScheduleParallel, Job<Lattice2D<Perlin>>.ScheduleParallel,
                Job<Lattice3D<Perlin>>.ScheduleParallel
            },
            {
                Job<Lattice1D<Value>>.ScheduleParallel, Job<Lattice2D<Value>>.ScheduleParallel, Job<Lattice3D<Value>>
                    .ScheduleParallel
            }
        };

        public enum NoiseType
        {
            Perlin,
            Value
        }

        private static int noiseId = Shader.PropertyToID("_Noise");

        [SerializeField] private int seed;
        [SerializeField] private SpaceTRS domain = new SpaceTRS { scale = 8f };
        [SerializeField, Range(1, 3)] private int dimensions = 3;
        [SerializeField] private NoiseType type;

        private NativeArray<float4> _noise;

        private ComputeBuffer _noiseBuffer;

        private static Shapes.ScheduleDelegate[] shapeJobs =
        {
            Shapes.Job<Shapes.Plane>.ScheduleParallel, Shapes.Job<Shapes.Sphere>.ScheduleParallel,
            Shapes.Job<Shapes.Torus>.ScheduleParallel,
        };

        struct HashJob : IJobFor
        {
            [ReadOnly] public NativeArray<float3x4> positions;

            [WriteOnly] public NativeArray<uint4> hashes;

            public SmallXXHash4 hash;

            public float3x4 domainTRS;

            float4x3 TransfromPositions(float3x4 trs, float4x3 p) => float4x3(
                trs.c0.x * p.c0 + trs.c1.x * p.c1 + trs.c2.x * p.c2 + trs.c3.x,
                trs.c0.y * p.c0 + trs.c1.y * p.c1 + trs.c2.y * p.c2 + trs.c3.y,
                trs.c0.z * p.c0 + trs.c1.z * p.c1 + trs.c2.z * p.c2 + trs.c3.z
            );

            public void Execute(int i)
            {
                float4x3 p = TransfromPositions(domainTRS, transpose(positions[i]));

                int4 u = (int4)floor(p.c0);
                int4 v = (int4)floor(p.c1);
                int4 w = (int4)floor(p.c2);

                hashes[i] = hash.Eat(u).Eat(v).Eat(w);
            }
        }

        protected override void EnableVisualisation(int dataLength, MaterialPropertyBlock propertyBlock)
        {
            _noise = new NativeArray<float4>(dataLength, Allocator.Persistent);
            _noiseBuffer = new ComputeBuffer(dataLength * 4, 4);
            propertyBlock.SetBuffer(noiseId, _noiseBuffer);
        }

        protected override void DisableVisualisation()
        {
            _noise.Dispose();
            _noiseBuffer.Release();
            _noiseBuffer = null;
        }

        protected override void UpdateVisualisation(NativeArray<float3x4> positions, int resolution, JobHandle handle)
        {
            noiseJobs[(int)type, dimensions - 1](positions, _noise, seed, domain, resolution, handle).Complete();
            _noiseBuffer.SetData(_noise.Reinterpret<float4>(4 * 4));
        }
    }
}