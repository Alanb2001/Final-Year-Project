using System;
using UnityEngine;

namespace VoxelTerrain
{
    public class TerrainGen : MonoBehaviour
    {
        [Serializable]
        public struct TerrainSettings
        {
            public float caveFrequency;

            public int caveSize;

            public int caveMax;
            public static TerrainSettings Default => new TerrainSettings
            {
                caveFrequency = 0.025f,

                caveSize = 20,
                
                caveMax = 60
            };
        }

        [SerializeField] private TerrainSettings settings = TerrainSettings.Default;

        public Chunk ChunkGen(Chunk chunk)
        {
            for (var x = chunk.pos.x - 3; x < chunk.pos.x + Chunk.ChunkSize + 3; x++)
            {
                for (var y = chunk.pos.y - 3; y < chunk.pos.y + Chunk.ChunkSize + 3; y++)
                {
                    for (var z = chunk.pos.z - 3; z < chunk.pos.z + Chunk.ChunkSize + 3; z++)
                    {
                        chunk = ChunkColumnGen(chunk, x, y, z);
                    }
                }
            }

            return chunk;
        }

        private Chunk ChunkColumnGen(Chunk chunk, int x, int y, int z)
        {
            // Get a value to base cave generation on
            var caveChance = GetNoise(x, y, z, settings.caveFrequency, settings.caveMax);
            SetBlock(x, y, z, settings.caveSize < caveChance ? new Block() : new BlockAir(), chunk);

            return chunk;
        }

        private static void SetBlock(int x, int y, int z, Block block, Chunk chunk, bool replaceBlocks = false)
        {
            x -= chunk.pos.x;
            y -= chunk.pos.y;
            z -= chunk.pos.z;
            
            if (!Chunk.InRange(x) || !Chunk.InRange(y) || !Chunk.InRange(z)) return;
            
            if (replaceBlocks || chunk.blocks[x, y, z] == null)
            {
                chunk.SetBlock(x, y, z, block);
            }
        }

        private int GetNoise(int x, int y, int z, float scale, int max)
        {
            return Mathf.FloorToInt((Noise.Simplex3D(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
        }
    }
}
