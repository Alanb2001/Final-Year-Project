using System;
using System.Collections.Generic;

namespace VoxelTerrain
{
    [Serializable]
    public class Save
    {
        public Dictionary<WorldPos, Block> blocks = new Dictionary<WorldPos, Block>();

        public Save(Chunk chunk)
        {
            for (var x = 0; x < Chunk.ChunkSize; x++)
            {
                for (var y = 0; y < Chunk.ChunkSize; y++)
                {
                    for (var z = 0; z < Chunk.ChunkSize; z++)
                    {
                        if (!chunk.blocks[x, y, z].changed)
                        {
                            continue;
                        }

                        var pos = new WorldPos(x, y, z);
                        blocks.Add(pos, chunk.blocks[x, y, z]);
                    }
                }
            }
        }
    }
}
