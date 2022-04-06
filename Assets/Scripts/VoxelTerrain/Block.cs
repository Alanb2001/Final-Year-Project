using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace VoxelTerrain
{
    [Serializable]
    public class Block
    {
        [BurstCompile]
        public struct BuildBlock : IJob
        {
            [ReadOnly] public int chunkSize;
            
            [WriteOnly] public NativeList<float3> vertices;
            [WriteOnly] public NativeList<int> triangles;
            [WriteOnly] public NativeList<float2> uvs;
            
            public void Execute()
            {
                BuildChunk();
            }

            private void BuildChunk()
            {
                for (int x = 0; x < chunkSize; x++)
                {
                    for (int y = 0; y < chunkSize; y++)
                    {
                        for (int z = 0; z < chunkSize; z++)
                        {
                            if (z < chunkSize - 1 || z == chunkSize - 1)
                            {
                                vertices.Add(new float3(x - 0.5f, y + 0.5f, z + 0.5f));
                                vertices.Add(new float3(x + 0.5f, y + 0.5f, z + 0.5f));
                                vertices.Add(new float3(x + 0.5f, y + 0.5f, z - 0.5f));
                                vertices.Add(new float3(x - 0.5f, y + 0.5f, z - 0.5f));
                            }
                        }
                    }
                }
            }
        }

        private const float TileSize = 0.25f;

        public bool changed = true;

        public virtual Vector2[] FaceUVs(Direction direction)
        {
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

        public virtual Tile TexturePosition(Direction direction)
        {
            var tile = new Tile
            {
                x = 0,
                y = 0
            };
            
            return tile;
        }
        
        public enum Direction
        {
            North,
            East,
            South,
            West,
            Up,
            Down
        }

        public virtual bool IsSolid(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return true;
                case Direction.East:
                    return true;
                case Direction.South:
                    return true;
                case Direction.West:
                    return true;
                case Direction.Up:
                    return true;
                case Direction.Down:
                    return true;
            }
            return false;
        }
        
        public virtual MeshData BlockData(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.useRenderDataForCol = true;
            if (!chunk.GetBlock(x, y + 1, z).IsSolid(Direction.Down))
            {
                meshData = FaceDataUp(chunk, x, y, z, meshData);
            }
            if (!chunk.GetBlock(x, y - 1, z).IsSolid(Direction.Up))
            {
                meshData = FaceDataDown(chunk, x, y, z, meshData);
            }
            if (!chunk.GetBlock(x, y, z + 1).IsSolid(Direction.South))
            {
                meshData = FaceDataNorth(chunk, x, y, z, meshData);
            }
            if (!chunk.GetBlock(x, y, z - 1).IsSolid(Direction.North))
            {
                meshData = FaceDataSouth(chunk, x, y, z, meshData);
            }
            if (!chunk.GetBlock(x + 1, y, z).IsSolid(Direction.West))
            {
                meshData = FaceDataEast(chunk, x, y, z, meshData);
            }
            if (!chunk.GetBlock(x - 1, y, z).IsSolid(Direction.East))
            {
                meshData = FaceDataWest(chunk, x, y, z, meshData);
            }
            return meshData;
        }

        protected virtual MeshData FaceDataUp(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
            
            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.Up));
            return meshData;
        }
        
        protected virtual MeshData FaceDataDown(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
            
            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.Down));
            return meshData;
        }
        
        protected virtual MeshData FaceDataNorth(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
            
            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.North));
            return meshData;
        }
        
        protected virtual MeshData FaceDataEast(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
            
            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.East));
            return meshData;
        }
        
        protected virtual MeshData FaceDataSouth(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
            
            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.South));
            return meshData;
        }
        
        protected virtual MeshData FaceDataWest(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
            
            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.West));
            return meshData;
        }
    }
}
