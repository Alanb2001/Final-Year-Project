using UnityEngine;

namespace VoxelTerrain
{
    public class TerrainGen
    {
        private float caveFrequency = 0.025f;
        
        private int caveSize = 20;

        public Chunk ChunkGen(Chunk chunk)
        {
            for (int x = chunk.pos.x - 3; x < chunk.pos.x + Chunk.chunkSize + 3; x++)
            {
                for (int z = chunk.pos.z - 3; z < chunk.pos.z + Chunk.chunkSize + 3; z++)
                {
                    chunk = ChunkColumnGen(chunk, x, z);
                }
            }

            return chunk;
        }

        public Chunk ChunkColumnGen(Chunk chunk, int x, int z)
        { 
           for (int y = chunk.pos.y - 8; y < chunk.pos.y + Chunk.chunkSize; y++)
           {
               // Get a value to base cave generation on
               int caveChance = GetNoise(x, y, z, caveFrequency, 60);
               if (caveSize < caveChance)
               {
                   SetBlock(x, y, z, new Block(), chunk);
               }
               else
               {
                   SetBlock(x, y, z, new BlockAir(), chunk);
               }
           }
            
           return chunk;
        }

        public static void SetBlock(int x, int y, int z, Block block, Chunk chunk, bool replaceBlocks = false)
        {
            x -= chunk.pos.x;
            y -= chunk.pos.y;
            z -= chunk.pos.z;
            if (Chunk.InRange(x) && Chunk.InRange(y) && Chunk.InRange(z))
            {
                if (replaceBlocks || chunk._blocks[x, y, z] == null)
                {
                    chunk.SetBlock(x, y, z, block);
                }
            }
        }

        public static int GetNoise(int x, int y, int z, float scale, int max)
        {
            return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
        }
    }
}
