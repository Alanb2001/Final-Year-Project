using System;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace VoxelTerrain
{
    //public struct ChunkJob : IJob
    //{
    //    public struct MeshData
    //    {
    //        public NativeList<float3> Vertices { get; set; }
    //        public NativeList<int> Triangles { get; set; }
    //    }
    //    
    //    public struct BlockData
    //    {
    //        public NativeArray<float3> Vertices { get; set; }
    //        public NativeArray<int> Triangles { get; set; }
    //    }
    //    
    //    public struct ChunkData
    //    {
    //        public NativeArray<Blocks> Blocks { get; set; }
    //    }
    //
    //    [WriteOnly] public MeshData meshData;
    //
    //    [ReadOnly] public ChunkData chunkData;
    //
    //    [ReadOnly] public BlockData blockData;
    //
    //    private int vCount;
//
    //    public void Execute()
    //    {
//
    //        for (var x = 0; x < Chunk.ChunkSize; x++)
    //        {
    //            for (var z = 0; z < Chunk.ChunkSize; z++)
    //            {
    //                for (var y = 0; y < Chunk.ChunkSize; y++)
    //                {
    //                    if (chunkData.Blocks[BlockExtensions.GetBlockIndex(new int3(x, y, z))].IsEmpty())
    //                    {
    //                        continue;
    //                    }
    //        
    //                    //BlockExtensions.BlockInfo(chunk, x, y, z, mesh);
    //        
    //                    for (var i = 0; i < 6; i++)
    //                    {
    //                        var direction = (Block.Direction)i;
    //        
    //                        if (Check(BlockExtensions.GetPositionInDirection(direction, x, y, z)))
    //                        {
    //                            CreateFace(direction, new int3(x, y, z));
    //                        }
    //                    }
    //                }
    //            }  
    //        }
    //    }
    //    
    //    private void CreateFace(Block.Direction direction, int3 pos)
    //    {
    //        var vertices = GetFaceVertices(direction, 1, pos);
    //            
    //        meshData.Vertices.AddRange(vertices);
    //
    //        vertices.Dispose();
    //
    //        vCount += 4;
    //
    //        meshData.Triangles.Add(vCount - 4);
    //        meshData.Triangles.Add(vCount - 4 + 1);
    //        meshData.Triangles.Add(vCount - 4 + 2);
    //        meshData.Triangles.Add(vCount - 4);
    //        meshData.Triangles.Add(vCount - 4 + 2);
    //        meshData.Triangles.Add(vCount - 4 + 3);
    //    }
    //
    //    private bool Check(int3 position)
    //    {
    //        if (position.x >= 16 || position.z >= 16 || position.x <= -1 || position.z <= -1 || position.y <= -1)
    //        {
    //            return true;
    //        }
    //
    //        if (position.y >= 16)
    //        {
    //            return false;
    //        }
    //
    //        return chunkData.Blocks[BlockExtensions.GetBlockIndex(position)].IsEmpty();
    //    }
    //    
    //    public NativeArray<float3> GetFaceVertices(Block.Direction direction, int scale, int3 pos)
    //    {
    //        var faceVertices = new NativeArray<float3>(4, Allocator.Temp);
    //
    //        for (int i = 0; i < 4; i++)
    //        {
    //            var index = blockData.Triangles[(int)direction * 4 + i];
    //            faceVertices[i] = blockData.Vertices[index] * scale + pos;
    //        }
    //
    //        return faceVertices;
    //    }
    //}
    
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class Chunk : MonoBehaviour
    {
        public readonly Block[,,] blocks = new Block[ChunkSize, ChunkSize, ChunkSize];
        public const int ChunkSize = 16;
        public bool update;
        public World world;
        public WorldPos pos;
        public bool rendered;

        private MeshFilter _filter;
        private MeshCollider _coll;

        // Use this for initialisation.
        private void Start()
        {
            _filter = gameObject.GetComponent<MeshFilter>();
            _coll = gameObject.GetComponent<MeshCollider>();
            
            // var position = transform.position;
            //
            // var blocks = new NativeArray<Blocks>(4096, Allocator.TempJob);
            //
            // for (int x = 0; x < ChunkSize; x++)
            // {
            //     for (int y = 0; y < ChunkSize; y++)
            //     {
            //         for (int z = 0; z < ChunkSize; z++)
            //         {
            //             var noise = Mathf.FloorToInt(
            //                 Mathf.PerlinNoise((position.x + x) * 0.15f, (position.z + z) * 0.15f) *
            //                 ChunkSize);
            //                 
            //             for (int i = 0; i < noise; i++)
            //             {
            //                 blocks[BlockExtensions.GetBlockIndex(new int3(x, i, z))] = Blocks.Stone;
            //             }
            //
            //             for (int i = noise; i < ChunkSize; i++)
            //             {
            //                 blocks[BlockExtensions.GetBlockIndex(new int3(x, i, z))] = Blocks.Air;
            //             }
            //         }
            //     }
            // }
            // 
            // var meshData = new ChunkJob.MeshData
            // {
            //     Vertices = new NativeList<float3>(Allocator.TempJob),
            //     Triangles = new NativeList<int>(Allocator.TempJob)
            // };
            //
            // var jobHandle = new ChunkJob
            // {
            //     meshData = meshData,
            //     chunkData = new ChunkJob.ChunkData
            //     {
            //         Blocks = blocks
            //     },
            //     blockData = new ChunkJob.BlockData
            //     {
            //         Vertices = BlockData.Vertices,
            //         Triangles = BlockData.Triangles
            //     }
            // }.Schedule();
            // 
            // jobHandle.Complete();
            // 
            // var mesh = new Mesh
            // {
            //     vertices = meshData.Vertices.ToArray().Select(vertex => new Vector3(vertex.x, vertex.y, vertex.z)).ToArray(),
            //     triangles = meshData.Triangles.ToArray()
            // };
//
            // meshData.Vertices.Dispose();
            // meshData.Triangles.Dispose();
            // blocks.Dispose();
            // 
            // mesh.RecalculateNormals();
            // mesh.RecalculateBounds();
            // mesh.RecalculateTangents();
            // 
            // _filter.mesh = mesh;
        }

        // Update is called once per frame
        private void Update()
        {
            if (!update) return;

            update = false;
            UpdateChunk();
        }

        public void SetBlocksUnmodified()
        {
            foreach (var block in blocks)
            {
                block.changed = false;
            }
        }

        public Block GetBlock(int x, int y, int z)
        {
            if (InRange(x) && InRange(y) && InRange(z))
            {
                return blocks[x, y, z];
            }

            return world.GetBlock(pos.x + x, pos.y + y, pos.z + z);
        }

        public static bool InRange(int index)
        {
            return index >= 0 && index < ChunkSize;
        }

        public void SetBlock(int x, int y, int z, Block block)
        {
            if (InRange(x) && InRange(y) && InRange(z))
            {
                blocks[x, y, z] = block;
            }
            else
            {
                world.SetBlock(pos.x + x, pos.y + y, pos.z + z, block);
            }
        }

        // Updates the chunk based on its contents
        private void UpdateChunk()
        {
            rendered = true;
            var meshData = new MeshData();
            for (var x = 0; x < ChunkSize; x++)
            {
                for (var y = 0; y < ChunkSize; y++)
                {
                    for (var z = 0; z < ChunkSize; z++)
                    {
                        meshData = blocks[x, y, z].BlockData(this, x, y, z, meshData);
                    }
                }
            }

            RenderMesh(meshData);
        }

        // Sends the calculated mesh information
        // to the mesh and collision components
        private void RenderMesh(MeshData meshData)
        {
            _filter.mesh.Clear();
            _filter.mesh.vertices = meshData.vertices.ToArray()
                .Select(vertex => new Vector3(vertex.x, vertex.y, vertex.z)).ToArray();
            _filter.mesh.triangles = meshData.triangles.ToArray();
            _filter.mesh.uv = meshData.uv.ToArray().Select(vertex => new Vector2(vertex.x, vertex.y)).ToArray();
            _filter.mesh.RecalculateNormals();

            _coll.sharedMesh = null;

            var mesh = new Mesh
            {
                vertices = meshData.colVertices.ToArray().Select(vertex => new Vector3(vertex.x, vertex.y, vertex.z)).ToArray(),
                triangles = meshData.colTriangles.ToArray()
            };

            mesh.RecalculateNormals();

            _coll.sharedMesh = mesh;
        }
    }
}