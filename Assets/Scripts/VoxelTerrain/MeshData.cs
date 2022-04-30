using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace VoxelTerrain
{
    public struct CreateMeshData : IJob
    { 
        [ReadOnly] public NativeArray<Blocks> blocks;

        public NativeList<float3> blockVerticles;
        public NativeList<int> blockTriangles;
        public NativeList<float2> blockUVs;

        public float TileSize;
        
        public void Execute()
        {
            NativeArray<float3> verts = new NativeArray<float3>(4, Allocator.Temp);
            NativeArray<float2> uv = new NativeArray<float2>(4, Allocator.Temp);
            NativeArray<int> triangles = new NativeArray<int>(6, Allocator.Temp);

            NativeArray<bool> drawFace = new NativeArray<bool>(6, Allocator.Temp);
            NativeArray<bool> flipFace = new NativeArray<bool>(6, Allocator.Temp);

            for (int x = 1; x < Chunk.ChunkSize + 1; x++)
            {
                for (int z = 1; z < Chunk.ChunkSize + 1; z++)
                {
                    for (int y = 1; y < Chunk.ChunkSize; y++)
                    {
                        int numFaces = 0;
                        
                        // assign default values
                        ref var verticles = ref blockVerticles;
                        ref var uvs = ref blockUVs;
                        ref var tris = ref blockTriangles;

                        for (int i = 0; i < 6; i++)
                        {
                            if (!drawFace[i])
                                continue;
                            verticles.AddRange(verts);
                            uvs.AddRange(uv);
                            
                            numFaces++;
                        }

                        // triangles
                        int tl = verticles.Length - 4 * numFaces;
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

                            tris.AddRange(triangles);
                        }
                    }
                }
            }

            // dispose native arrays
            verts.Dispose();
            uv.Dispose();
            triangles.Dispose();

            drawFace.Dispose();
            flipFace.Dispose();
        }
        
        public  Vector2[] FaceUVs(Block.Direction direction)
        {
            TileSize = 0.25f;
            var uVs = new Vector2[4];
            var tilePos = TexturePosition(direction);
            uVs[0] = new Vector2(TileSize * tilePos.x + TileSize, TileSize * tilePos.y);
            uVs[1] = new Vector2(TileSize * tilePos.x + TileSize, TileSize * tilePos.y + TileSize);
            uVs[2] = new Vector2(TileSize * tilePos.x, TileSize * tilePos.y + TileSize);
            uVs[3] = new Vector2(TileSize * tilePos.x, TileSize * tilePos.y);
            return uVs;
        }

        public struct Tile
        {
            public int x;
            public int y;
        }

        public  Tile TexturePosition(Block.Direction direction)
        {
            var tile = new Tile
            {
                x = 0,
                y = 0
            };

            return tile;
        }
        
        private MeshData BlockData(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.useRenderDataForCol = true;
            if (!chunk.GetBlock(x, y + 1, z).IsSolid(Block.Direction.Down))
            {
                meshData = FaceDataUp(chunk, x, y, z, meshData);
            }

            if (!chunk.GetBlock(x, y - 1, z).IsSolid(Block.Direction.Up))
            {
                meshData = FaceDataDown(chunk, x, y, z, meshData);
            }

            if (!chunk.GetBlock(x, y, z + 1).IsSolid(Block.Direction.South))
            {
                meshData = FaceDataNorth(chunk, x, y, z, meshData);
            }

            if (!chunk.GetBlock(x, y, z - 1).IsSolid(Block.Direction.North))
            {
                meshData = FaceDataSouth(chunk, x, y, z, meshData);
            }

            if (!chunk.GetBlock(x + 1, y, z).IsSolid(Block.Direction.West))
            {
                meshData = FaceDataEast(chunk, x, y, z, meshData);
            }

            if (!chunk.GetBlock(x - 1, y, z).IsSolid(Block.Direction.East))
            {
                meshData = FaceDataWest(chunk, x, y, z, meshData);
            }

            return meshData;
        }

        private MeshData FaceDataUp(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));

            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Block.Direction.Up));
            return meshData;
        }

        private MeshData FaceDataDown(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Block.Direction.Down));
            return meshData;
        }

        private MeshData FaceDataNorth(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Block.Direction.North));
            return meshData;
        }

        private MeshData FaceDataEast(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));

            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Block.Direction.East));
            return meshData;
        }

        private MeshData FaceDataSouth(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));

            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Block.Direction.South));
            return meshData;
        }

        private MeshData FaceDataWest(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));

            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Block.Direction.West));
            return meshData;
        }
    }
    
    public struct CubeJob : IJob
    {
        public struct MeshData
        {
            public NativeList<int3> Vertices { get; set; }
            public NativeList<int> Triangles { get; set; }
        }
    
        public int3 position { get; set; }
        
        public MeshData meshData { get; set; }
        
        public void Execute()
        {
            for (int i = 0; i < 6; i++)
            {
                CreateFace((Block.Direction)i, position);
            }
        }
        
        private void CreateFace(Block.Direction direction, int3 pos)
        {
            var _vertices = BlockExtensions.GetFaceVertices(direction, 1, pos);
                
            meshData.Vertices.AddRange(_vertices);
    
            _vertices.Dispose();
            
            var vCount = meshData.Vertices.Length - 4;
    
            meshData.Triangles.Add(vCount);
            meshData.Triangles.Add(vCount+ 1);
            meshData.Triangles.Add(vCount + 2);
            meshData.Triangles.Add(vCount);
            meshData.Triangles.Add(vCount + 2);
            meshData.Triangles.Add(vCount + 3);
        }
    }
    public class Cube : MonoBehaviour
    {
        private MeshFilter _meshFilter;
        private JobHandle _jobHandle;
        private CubeJob.MeshData _meshData;

        private bool _jobCompleted;
        
        private void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();
        }
    
        private void Start()
        {
            var meshData = new CubeJob.MeshData
            {
                Vertices = new NativeList<int3>(Allocator.TempJob),
                Triangles = new NativeList<int>(Allocator.TempJob)
            };
    
            var cubeJob = new CubeJob()
            {
                position = int3.zero,
                meshData = meshData
            };
    
            var jobHandle = cubeJob.Schedule();
        }
    
        private void Update()
        {
            if (!_jobCompleted && _jobHandle.IsCompleted)
            {
                _jobCompleted = true;
                OnComplete();
            }
        }

        private void OnComplete()
        {
            _jobHandle.Complete();
            
            var mesh = new Mesh()
            {
                vertices = _meshData.Vertices.ToArray().Select(vertex => new Vector3(vertex.x, vertex.y, vertex.z)).ToArray(),
                triangles = _meshData.Triangles.ToArray()
            };
    
            _meshData.Vertices.Dispose();
            _meshData.Triangles.Dispose();

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.RecalculateTangents();
            
            _meshFilter.mesh = mesh;
        }
    }
    public class MeshData
    {
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
