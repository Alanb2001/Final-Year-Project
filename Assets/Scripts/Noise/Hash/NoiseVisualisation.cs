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
                Job<Lattice1D<LatticeNormal, Perlin>>.ScheduleParallel,
                Job<Lattice1D<LatticeTiling, Perlin>>.ScheduleParallel,
                Job<Lattice2D<LatticeNormal, Perlin>>.ScheduleParallel,
                Job<Lattice2D<LatticeTiling, Perlin>>.ScheduleParallel,
                Job<Lattice3D<LatticeNormal, Perlin>>.ScheduleParallel,
                Job<Lattice3D<LatticeTiling, Perlin>>.ScheduleParallel
            },
            {
                Job<Lattice1D<LatticeNormal, Turbulence<Perlin>>>.ScheduleParallel,
                Job<Lattice1D<LatticeTiling, Turbulence<Perlin>>>.ScheduleParallel,
                Job<Lattice2D<LatticeNormal, Turbulence<Perlin>>>.ScheduleParallel,
                Job<Lattice2D<LatticeTiling, Turbulence<Perlin>>>.ScheduleParallel,
                Job<Lattice3D<LatticeNormal, Turbulence<Perlin>>>.ScheduleParallel,
                Job<Lattice3D<LatticeTiling, Turbulence<Perlin>>>.ScheduleParallel
            },
            {
                Job<Lattice1D<LatticeNormal, Value>>.ScheduleParallel,
                Job<Lattice1D<LatticeTiling, Value>>.ScheduleParallel, 
                Job<Lattice2D<LatticeNormal, Value>>.ScheduleParallel, 
                Job<Lattice2D<LatticeTiling, Value>>.ScheduleParallel,
                Job<Lattice3D<LatticeNormal, Value>>.ScheduleParallel,
                Job<Lattice3D<LatticeTiling, Value>>.ScheduleParallel
            },
            {
                Job<Lattice1D<LatticeNormal, Turbulence<Value>>>.ScheduleParallel,
                Job<Lattice1D<LatticeTiling, Turbulence<Value>>>.ScheduleParallel, 
                Job<Lattice2D<LatticeNormal, Turbulence<Value>>>.ScheduleParallel,
                Job<Lattice2D<LatticeTiling, Turbulence<Value>>>.ScheduleParallel,
                Job<Lattice3D<LatticeNormal, Turbulence<Value>>>.ScheduleParallel,
                Job<Lattice3D<LatticeTiling, Turbulence<Value>>>.ScheduleParallel
            },
            {
                Job<Simplex1D<Simplex>>.ScheduleParallel,
                Job<Simplex1D<Simplex>>.ScheduleParallel, 
                Job<Simplex2D<Simplex>>.ScheduleParallel, 
                Job<Simplex2D<Simplex>>.ScheduleParallel,
                Job<Simplex3D<Simplex>>.ScheduleParallel,
                Job<Simplex3D<Simplex>>.ScheduleParallel
            },
            {
                Job<Simplex1D<Turbulence<Simplex>>>.ScheduleParallel,
                Job<Simplex1D<Turbulence<Simplex>>>.ScheduleParallel, 
                Job<Simplex2D<Turbulence<Simplex>>>.ScheduleParallel,
                Job<Simplex2D<Turbulence<Simplex>>>.ScheduleParallel,
                Job<Simplex3D<Turbulence<Simplex>>>.ScheduleParallel,
                Job<Simplex3D<Turbulence<Simplex>>>.ScheduleParallel
            },
            {
                Job<Simplex1D<Value>>.ScheduleParallel,
                Job<Simplex1D<Value>>.ScheduleParallel, 
                Job<Simplex2D<Value>>.ScheduleParallel, 
                Job<Simplex2D<Value>>.ScheduleParallel,
                Job<Simplex3D<Value>>.ScheduleParallel,
                Job<Simplex3D<Value>>.ScheduleParallel
            },
            {
                 Job<Simplex1D<Turbulence<Value>>>.ScheduleParallel,
                 Job<Simplex1D<Turbulence<Value>>>.ScheduleParallel, 
                 Job<Simplex2D<Turbulence<Value>>>.ScheduleParallel,
                 Job<Simplex2D<Turbulence<Value>>>.ScheduleParallel,
                 Job<Simplex3D<Turbulence<Value>>>.ScheduleParallel,
                 Job<Simplex3D<Turbulence<Value>>>.ScheduleParallel
            },
            {
                Job<Voronoi1D<LatticeNormal, Worley, F1>>.ScheduleParallel,
                Job<Voronoi1D<LatticeTiling, Worley, F1>>.ScheduleParallel, 
                Job<Voronoi2D<LatticeNormal, Worley, F1>>.ScheduleParallel,
                Job<Voronoi2D<LatticeTiling, Worley, F1>>.ScheduleParallel,
                Job<Voronoi3D<LatticeNormal, Worley, F1>>.ScheduleParallel,
                Job<Voronoi3D<LatticeTiling, Worley, F1>>.ScheduleParallel
            },
            {
                Job<Voronoi1D<LatticeNormal, Worley, F2>>.ScheduleParallel,
                Job<Voronoi1D<LatticeTiling, Worley, F2>>.ScheduleParallel, 
                Job<Voronoi2D<LatticeNormal, Worley, F2>>.ScheduleParallel,
                Job<Voronoi2D<LatticeTiling, Worley, F2>>.ScheduleParallel,
                Job<Voronoi3D<LatticeNormal, Worley, F2>>.ScheduleParallel,
                Job<Voronoi3D<LatticeTiling, Worley, F2>>.ScheduleParallel
            },
            {
                Job<Voronoi1D<LatticeNormal, Worley, F2MinusF1>>.ScheduleParallel,
                Job<Voronoi1D<LatticeTiling, Worley, F2MinusF1>>.ScheduleParallel, 
                Job<Voronoi2D<LatticeNormal, Worley, F2MinusF1>>.ScheduleParallel,
                Job<Voronoi2D<LatticeTiling, Worley, F2MinusF1>>.ScheduleParallel,
                Job<Voronoi3D<LatticeNormal, Worley, F2MinusF1>>.ScheduleParallel,
                Job<Voronoi3D<LatticeTiling, Worley, F2MinusF1>>.ScheduleParallel
            },
            {
                Job<Voronoi1D<LatticeNormal, Worley, F1>>.ScheduleParallel,
                Job<Voronoi1D<LatticeTiling, Worley, F1>>.ScheduleParallel, 
                Job<Voronoi2D<LatticeNormal, Chebyshev, F1>>.ScheduleParallel,
                Job<Voronoi2D<LatticeTiling, Chebyshev, F1>>.ScheduleParallel,
                Job<Voronoi3D<LatticeNormal, Chebyshev, F1>>.ScheduleParallel,
                Job<Voronoi3D<LatticeTiling, Chebyshev, F1>>.ScheduleParallel
            },
            {
                Job<Voronoi1D<LatticeNormal, Worley, F2>>.ScheduleParallel,
                Job<Voronoi1D<LatticeTiling, Worley, F2>>.ScheduleParallel, 
                Job<Voronoi2D<LatticeNormal, Chebyshev, F2>>.ScheduleParallel,
                Job<Voronoi2D<LatticeTiling, Chebyshev, F2>>.ScheduleParallel,
                Job<Voronoi3D<LatticeNormal, Chebyshev, F2>>.ScheduleParallel,
                Job<Voronoi3D<LatticeTiling, Chebyshev, F2>>.ScheduleParallel
            },
            {
                Job<Voronoi1D<LatticeNormal, Worley, F2MinusF1>>.ScheduleParallel,
                Job<Voronoi1D<LatticeTiling, Worley, F2MinusF1>>.ScheduleParallel, 
                Job<Voronoi2D<LatticeNormal, Chebyshev, F2MinusF1>>.ScheduleParallel,
                Job<Voronoi2D<LatticeTiling, Chebyshev, F2MinusF1>>.ScheduleParallel,
                Job<Voronoi3D<LatticeNormal, Chebyshev, F2MinusF1>>.ScheduleParallel,
                Job<Voronoi3D<LatticeTiling, Chebyshev, F2MinusF1>>.ScheduleParallel
            }
        };

        public enum NoiseType
        {
            Perlin,
            PerlinTurbulence,
            Value,
            ValueTurbulence,
            Simplex,
            SimplexTurbulence,
            SimplexValue,
            SimplexValueTurbulence,
            VoronoiWorleyF1,
            VoronoiWorleyF2,
            VoronoiWorleyF2MinusF1,
            VoronoiChebyshevF1,
            VoronoiChebyshevF2,
            VoronoiChebyshevF2MinusF1
        }

        private static int noiseId = Shader.PropertyToID("_Noise");

        [SerializeField] private Settings noiseSettings = Settings.Default;
        [SerializeField] private SpaceTRS domain = new SpaceTRS { scale = 8f };
        [SerializeField, Range(1, 3)] private int dimensions = 3;
        [SerializeField] private NoiseType type;
        [SerializeField] private bool tiling;
        
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
            noiseJobs[(int)type, 2 * dimensions - (tiling ? 1 : 2)](positions, _noise, noiseSettings, domain, resolution, handle).Complete();
            _noiseBuffer.SetData(_noise.Reinterpret<float4>(4 * 4));
        }
    }
}