using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace VoxelTerrain
{
   //public enum Blocks : ushort
   //{
   //    Air = 0x0001,
   //    Stone = 0x0002
   //}
   //
   //public struct BlockData
   //{
   //    [ReadOnly] public static NativeArray<float3> Vertices = new NativeArray<float3>(8, Allocator.Persistent)
   //    {
   //        [0] = new float3(1, 1, 1),
   //        [1] = new float3(0, 1, 1),
   //        [2] = new float3(0, 0, 1),
   //        [3] = new float3(1, 0, 1),
   //        [4] = new float3(0, 1, 0),
   //        [5] = new float3(1, 1, 0),
   //        [6] = new float3(1, 0, 0),
   //        [7] = new float3(0, 0, 0)
   //    };
   //    
   //    [ReadOnly] public static NativeArray<int> Triangles = new NativeArray<int>(24, Allocator.Persistent)
   //    {
   //        [0] = 0, [1] = 1, [2] = 2, [3] = 3,
   //        [4] = 5, [5] = 0, [6] = 3, [7] = 6,
   //        [8] = 4, [9] = 5, [10] = 6, [11] = 7,
   //        [12] = 1, [13] = 4, [14] = 7, [15] = 2,
   //        [16] = 5, [17] = 4, [18] = 1, [19] = 0,
   //        [20] = 3, [21] = 2, [22] = 7, [23] = 6
   //    };
   //}
   //
   //public static class BlockExtensions
   //{
   //    public static NativeArray<float3> GetFaceVertices(Block.Direction direction, int scale, int3 pos)
   //    {
   //        var faceVertices = new NativeArray<float3>(4, Allocator.Temp);

   //        for (int i = 0; i < 4; i++)
   //        {
   //            var index = BlockData.Triangles[(int)direction * 4 + i];
   //            faceVertices[i] = BlockData.Vertices[index] * scale + pos;
   //        }
   //
   //        return faceVertices;
   //    }
   //
   //    public static int GetBlockIndex(int3 position) => position.x + position.z * 16 + position.y * 16 * 16;
   //
   //    public static bool IsEmpty(this Blocks block) => block == Blocks.Air;
   //
   //    public static int3 GetPositionInDirection(Block.Direction direction, int x, int y, int z)
   //    {
   //        switch (direction)
   //        {
   //            case Block.Direction.North:
   //                return new int3(x, y, z + 1);
   //            case Block.Direction.East:
   //                return new int3(x + 1, y, z);
   //            case Block.Direction.South:
   //                return new int3(x, y, z - 1);
   //            case Block.Direction.West:
   //                return new int3(x - 1, y, z);
   //            case Block.Direction.Up:
   //                return new int3(x, y + 1, z);
   //            case Block.Direction.Down:
   //                return new int3(x, y - 1, z);
   //            default:
   //                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
   //        }
   //    }
   //}
    
    [Serializable]
    public class Block
    {
        private const float TileSize = 0.25f;

        public bool changed = true;

        public Vector2[] FaceUVs(Direction direction)
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

        public Tile TexturePosition(Direction direction)
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

        protected MeshData FaceDataUp(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));

            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.Up));
            return meshData;
        }

        protected MeshData FaceDataDown(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.Down));
            return meshData;
        }

        protected MeshData FaceDataNorth(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.North));
            return meshData;
        }

        protected MeshData FaceDataEast(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));

            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.East));
            return meshData;
        }

        protected MeshData FaceDataSouth(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));

            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.South));
            return meshData;
        }

        protected MeshData FaceDataWest(Chunk chunk, int x, int y, int z, MeshData meshData)
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