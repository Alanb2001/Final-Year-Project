using System;

namespace VoxelTerrain
{
    [Serializable]
    public class BlockAir : Block
    {
        public override MeshData BlockData(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            return meshData;
        }

        public override bool IsSolid(Direction direction)
        {
            return false;
        }
    }
}
