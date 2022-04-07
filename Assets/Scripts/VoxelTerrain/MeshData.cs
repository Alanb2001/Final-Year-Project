using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace VoxelTerrain
{
    public class MeshData
    {
        [BurstCompile]
        public struct BlockData : IJob
        {
            public NativeList<float3> vertices;
            public NativeList<int> triangles;
            public NativeList<float2> uv;
            public NativeList<float3> colVertices;
            public NativeList<int> colTriangles;
            public bool useRenderDataForCol;
            
            public void Execute()
            {
                NativeArray<float3> verts = new NativeArray<float3>(4, Allocator.Temp);
                NativeArray<int> tri = new NativeArray<int>(6, Allocator.Temp);
                NativeArray<float2> uvs = new NativeArray<float2>(4, Allocator.Temp);

                NativeArray<bool> drawFace = new NativeArray<bool>(6, Allocator.Temp);
                NativeArray<bool> flipFace = new NativeArray<bool>(6, Allocator.Temp);

                ref var refVert = ref vertices;
                ref var refTri = ref triangles;
                ref var refUVs = ref uv;

                var numFaces = 0;

                for (int i = 0; i < 6; i++)
                {
                    if (!drawFace[i])
                    {
                        continue;
                    }
                    
                    refVert.AddRange(verts);
                    refUVs.AddRange(uvs);

                    numFaces++;
                }
                

                int tl = refVert.Length - 4 * numFaces;
                for (int i = 0; i < numFaces; i++)
                {
                    if (flipFace[i])
                    {
                        triangles[5] = tl + i * 4;
                        triangles[4] = tl + i * 4 + 1;
                        triangles[3] = tl + i * 4 + 2;
                        triangles[2] = tl + i * 4;
                        triangles[1] = tl + i * 4 + 2;
                        triangles[0] = tl + i * 4 + 3;
                    }
                    else
                    {
                        triangles[0] = tl + i * 4;
                        triangles[1] = tl + i * 4 + 1;
                        triangles[2] = tl + i * 4 + 2;
                        triangles[3] = tl + i * 4;
                        triangles[4] = tl + i * 4 + 2;
                        triangles[5] = tl + i * 4 + 3;
                    }
                    
                    refTri.AddRange(triangles);
                }

                verts.Dispose();
                tri.Dispose();
                uvs.Dispose();

                drawFace.Dispose();
                flipFace.Dispose();
            }
            
            public void AddQuadTriangles()
            {
                triangles.Add(vertices.Length - 4);
                triangles.Add(vertices.Length - 3);
                triangles.Add(vertices.Length - 2);
                triangles.Add(vertices.Length - 4);
                triangles.Add(vertices.Length - 2);
                triangles.Add(vertices.Length - 1);
            
                if (!useRenderDataForCol) return;
            
                colTriangles.Add(vertices.Length - 4);
                colTriangles.Add(vertices.Length - 3);
                colTriangles.Add(vertices.Length - 2);
                colTriangles.Add(vertices.Length - 4);
                colTriangles.Add(vertices.Length - 2);
                colTriangles.Add(vertices.Length - 1);
            }

            public void AddVertex(float3 vertex)
            {
                vertices.Add(vertex);
                if (useRenderDataForCol)
                {
                    colVertices.Add(vertex);
                }
            }
        }
        
        public readonly List<Vector3> vertices = new List<Vector3>();
        public readonly List<int> triangles = new List<int>();
        public readonly List<Vector2> uv = new List<Vector2>();
        public readonly List<Vector3> colVertices = new List<Vector3>();
        public readonly List<int> colTriangles = new List<int>();
        public bool useRenderDataForCol;

        public void AddQuadTriangles()
        {
            triangles.Add(vertices.Count - 4);
            triangles.Add(vertices.Count - 3);
            triangles.Add(vertices.Count - 2);
            triangles.Add(vertices.Count - 4);
            triangles.Add(vertices.Count - 2);
            triangles.Add(vertices.Count - 1);
            
            if (!useRenderDataForCol) return;
            
            colTriangles.Add(vertices.Count - 4);
            colTriangles.Add(vertices.Count - 3);
            colTriangles.Add(vertices.Count - 2);
            colTriangles.Add(vertices.Count - 4);
            colTriangles.Add(vertices.Count - 2);
            colTriangles.Add(vertices.Count - 1);
        }

        public void AddVertex(Vector3 vertex)
        {
            vertices.Add(vertex);
            if (useRenderDataForCol)
            {
                colVertices.Add(vertex);
            }
        }
    }
}
