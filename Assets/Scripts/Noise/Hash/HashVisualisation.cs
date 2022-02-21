using Unity.Burst;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

using static Unity.Mathematics.math;

namespace Noise.Hash
{
    public class HashVisualisation : Visualisation
    {
        private static int hasesId = Shader.PropertyToID("_Hashes");

        [SerializeField] private int seed;
        [SerializeField] private SpaceTRS domain = new SpaceTRS { scale = 8f };

        private NativeArray<uint4> _hashes;

        private ComputeBuffer _hashesBuffer;

        private static Shapes.ScheduleDelegate[] shapeJobs =
        {
            Shapes.Job<Shapes.Plane>.ScheduleParallel, Shapes.Job<Shapes.Sphere>.ScheduleParallel,
            Shapes.Job<Shapes.Torus>.ScheduleParallel,
        };

        [BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
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
                float4x3 p = domainTRS.TransfromVectors(transpose(positions[i]));

                int4 u = (int4)floor(p.c0);
                int4 v = (int4)floor(p.c1);
                int4 w = (int4)floor(p.c2);

                hashes[i] = hash.Eat(u).Eat(v).Eat(w);
            }
        }

        protected override void EnableVisualisation(int dataLength, MaterialPropertyBlock propertyBlock)
        {
            _hashes = new NativeArray<uint4>(dataLength, Allocator.Persistent);
            _hashesBuffer = new ComputeBuffer(dataLength * 4, 4);
            propertyBlock.SetBuffer(hasesId, _hashesBuffer);
        }

        protected override void DisableVisualisation()
        {
            _hashes.Dispose();
            _hashesBuffer.Release();
            _hashesBuffer = null;
        }

        protected override void UpdateVisualisation(NativeArray<float3x4> positions, int resolution, JobHandle handle)
        {
            new HashJob
            {
                positions = positions, hashes = _hashes, hash = SmallXXHash.Seed(seed), domainTRS = domain.Matrix
            }.ScheduleParallel(_hashes.Length, resolution, handle).Complete();

            _hashesBuffer.SetData(_hashes.Reinterpret<uint>(4 * 4));
        }
    }
}