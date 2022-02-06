using System;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelTerrain
{
    public class LoadChunks : MonoBehaviour
    {
        public World world;
        private List<WorldPos> updateList = new List<WorldPos>();
        private List<WorldPos> buildList = new List<WorldPos>();

        private int timer = 0;
        
        static WorldPos[] chunkPositions =
        {
            new WorldPos(0, 0, 0), new WorldPos(-1, 0, 0), new WorldPos(0, 0, -1), new WorldPos(0, 0, 1),
            new WorldPos(1, 0, 0),
            new WorldPos(-1, 0, -1), new WorldPos(-1, 0, 1), new WorldPos(1, 0, -1), new WorldPos(1, 0, 1),
            new WorldPos(-2, 0, 0),
            new WorldPos(0, 0, -2), new WorldPos(0, 0, 2), new WorldPos(2, 0, 0), new WorldPos(-2, 0, -1),
            new WorldPos(-2, 0, 1),
            new WorldPos(-1, 0, -2), new WorldPos(-1, 0, 2), new WorldPos(1, 0, -2), new WorldPos(1, 0, 2),
            new WorldPos(2, 0, -1),
            new WorldPos(2, 0, 1), new WorldPos(-2, 0, -2), new WorldPos(-2, 0, 2), new WorldPos(2, 0, -2),
            new WorldPos(2, 0, 2),
            new WorldPos(-3, 0, 0), new WorldPos(0, 0, -3), new WorldPos(0, 0, 3), new WorldPos(3, 0, 0),
            new WorldPos(-3, 0, -1),
            new WorldPos(-3, 0, 1), new WorldPos(-1, 0, -3), new WorldPos(-1, 0, 3), new WorldPos(1, 0, -3),
            new WorldPos(1, 0, 3),
            new WorldPos(3, 0, -1), new WorldPos(3, 0, 1), new WorldPos(-3, 0, -2), new WorldPos(-3, 0, 2),
            new WorldPos(-2, 0, -3),
            new WorldPos(-2, 0, 3), new WorldPos(2, 0, -3), new WorldPos(2, 0, 3), new WorldPos(3, 0, -2),
            new WorldPos(3, 0, 2),
            new WorldPos(-4, 0, 0), new WorldPos(0, 0, -4), new WorldPos(0, 0, 4), new WorldPos(4, 0, 0),
            new WorldPos(-4, 0, -1),
            new WorldPos(-4, 0, 1), new WorldPos(-1, 0, -4), new WorldPos(-1, 0, 4), new WorldPos(1, 0, -4),
            new WorldPos(1, 0, 4),
            new WorldPos(4, 0, -1), new WorldPos(4, 0, 1), new WorldPos(-3, 0, -3), new WorldPos(-3, 0, 3),
            new WorldPos(3, 0, -3),
            new WorldPos(3, 0, 3), new WorldPos(-4, 0, -2), new WorldPos(-4, 0, 2), new WorldPos(-2, 0, -4),
            new WorldPos(-2, 0, 4),
            new WorldPos(2, 0, -4), new WorldPos(2, 0, 4), new WorldPos(4, 0, -2), new WorldPos(4, 0, 2),
            new WorldPos(-5, 0, 0),
            new WorldPos(-4, 0, -3), new WorldPos(-4, 0, 3), new WorldPos(-3, 0, -4), new WorldPos(-3, 0, 4),
            new WorldPos(0, 0, -5),
            new WorldPos(0, 0, 5), new WorldPos(3, 0, -4), new WorldPos(3, 0, 4), new WorldPos(4, 0, -3),
            new WorldPos(4, 0, 3),
            new WorldPos(5, 0, 0), new WorldPos(-5, 0, -1), new WorldPos(-5, 0, 1), new WorldPos(-1, 0, -5),
            new WorldPos(-1, 0, 5),
            new WorldPos(1, 0, -5), new WorldPos(1, 0, 5), new WorldPos(5, 0, -1), new WorldPos(5, 0, 1),
            new WorldPos(-5, 0, -2),
            new WorldPos(-5, 0, 2), new WorldPos(-2, 0, -5), new WorldPos(-2, 0, 5), new WorldPos(2, 0, -5),
            new WorldPos(2, 0, 5),
            new WorldPos(5, 0, -2), new WorldPos(5, 0, 2), new WorldPos(-4, 0, -4), new WorldPos(-4, 0, 4),
            new WorldPos(4, 0, -4),
            new WorldPos(4, 0, 4), new WorldPos(-5, 0, -3), new WorldPos(-5, 0, 3), new WorldPos(-3, 0, -5),
            new WorldPos(-3, 0, 5),
            new WorldPos(3, 0, -5), new WorldPos(3, 0, 5), new WorldPos(5, 0, -3), new WorldPos(5, 0, 3),
            new WorldPos(-6, 0, 0),
            new WorldPos(0, 0, -6), new WorldPos(0, 0, 6), new WorldPos(6, 0, 0), new WorldPos(-6, 0, -1),
            new WorldPos(-6, 0, 1),
            new WorldPos(-1, 0, -6), new WorldPos(-1, 0, 6), new WorldPos(1, 0, -6), new WorldPos(1, 0, 6),
            new WorldPos(6, 0, -1),
            new WorldPos(6, 0, 1), new WorldPos(-6, 0, -2), new WorldPos(-6, 0, 2), new WorldPos(-2, 0, -6),
            new WorldPos(-2, 0, 6),
            new WorldPos(2, 0, -6), new WorldPos(2, 0, 6), new WorldPos(6, 0, -2), new WorldPos(6, 0, 2),
            new WorldPos(-5, 0, -4),
            new WorldPos(-5, 0, 4), new WorldPos(-4, 0, -5), new WorldPos(-4, 0, 5), new WorldPos(4, 0, -5),
            new WorldPos(4, 0, 5),
            new WorldPos(5, 0, -4), new WorldPos(5, 0, 4), new WorldPos(-6, 0, -3), new WorldPos(-6, 0, 3),
            new WorldPos(-3, 0, -6),
            new WorldPos(-3, 0, 6), new WorldPos(3, 0, -6), new WorldPos(3, 0, 6), new WorldPos(6, 0, -3),
            new WorldPos(6, 0, 3),
            new WorldPos(-7, 0, 0), new WorldPos(0, 0, -7), new WorldPos(0, 0, 7), new WorldPos(7, 0, 0),
            new WorldPos(-7, 0, -1),
            new WorldPos(-7, 0, 1), new WorldPos(-5, 0, -5), new WorldPos(-5, 0, 5), new WorldPos(-1, 0, -7),
            new WorldPos(-1, 0, 7),
            new WorldPos(1, 0, -7), new WorldPos(1, 0, 7), new WorldPos(5, 0, -5), new WorldPos(5, 0, 5),
            new WorldPos(7, 0, -1),
            new WorldPos(7, 0, 1), new WorldPos(-6, 0, -4), new WorldPos(-6, 0, 4), new WorldPos(-4, 0, -6),
            new WorldPos(-4, 0, 6),
            new WorldPos(4, 0, -6), new WorldPos(4, 0, 6), new WorldPos(6, 0, -4), new WorldPos(6, 0, 4),
            new WorldPos(-7, 0, -2),
            new WorldPos(-7, 0, 2), new WorldPos(-2, 0, -7), new WorldPos(-2, 0, 7), new WorldPos(2, 0, -7),
            new WorldPos(2, 0, 7),
            new WorldPos(7, 0, -2), new WorldPos(7, 0, 2), new WorldPos(-7, 0, -3), new WorldPos(-7, 0, 3),
            new WorldPos(-3, 0, -7),
            new WorldPos(-3, 0, 7), new WorldPos(3, 0, -7), new WorldPos(3, 0, 7), new WorldPos(7, 0, -3),
            new WorldPos(7, 0, 3),
            new WorldPos(-6, 0, -5), new WorldPos(-6, 0, 5), new WorldPos(-5, 0, -6), new WorldPos(-5, 0, 6),
            new WorldPos(5, 0, -6),
            new WorldPos(5, 0, 6), new WorldPos(6, 0, -5), new WorldPos(6, 0, 5)
        };

        void FindChunksToLoad()
        {
            // Get the position of this gameobject to generate around
            WorldPos playerPos = new WorldPos(
                Mathf.FloorToInt(transform.position.x / Chunk.chunkSize) * Chunk.chunkSize,
                Mathf.FloorToInt(transform.position.y / Chunk.chunkSize) * Chunk.chunkSize,
                Mathf.FloorToInt(transform.position.z / Chunk.chunkSize) * Chunk.chunkSize);
            
            // If there aren't already chunks to generate
            if (buildList.Count == 0)
            {
                // Cycle through the array of positions
                for (int i = 0; i < chunkPositions.Length; i++)
                {
                    // Translate the player position and array position into chunk position
                    WorldPos newChunkPos = new WorldPos(chunkPositions[i].x * Chunk.chunkSize + playerPos.x, 0,
                        chunkPositions[i].z * Chunk.chunkSize + playerPos.z);
                    // Get the chunk in the defined position
                    Chunk newChunk = world.GetChunk(newChunkPos.x, newChunkPos.y, newChunkPos.z);
                    // If the chunk already exists and it's already
                    // rendered or in queue to be rendered continue
                    if (newChunk != null && (newChunk.rendered || updateList.Contains(newChunkPos)))
                    {
                        continue;
                    }
                    // Load a column of chunks in this position
                    for (int y = -4; y < 4; y++)
                    {
                        buildList.Add(new WorldPos(newChunkPos.x, y * Chunk.chunkSize, newChunkPos.z));
                    }
                    return;
                }
            }
        }

        void BuildChunk(WorldPos pos)
        {
            for (int y = pos.y - Chunk.chunkSize; y <= pos.y + Chunk.chunkSize; y += Chunk.chunkSize)
            {
                if (y > 64 || y < - 64)
                {
                    continue;
                }

                for (int x = pos.x - Chunk.chunkSize; x <= pos.x + Chunk.chunkSize; x += Chunk.chunkSize)
                {
                    for (int z = pos.z - Chunk.chunkSize; z <= pos.z + Chunk.chunkSize; z += Chunk.chunkSize)
                    {
                        if (world.GetChunk(x, y, z) == null)
                        {
                            world.CreateChunk(x, y, z);
                        }
                    }
                }
            }
            updateList.Add(pos);
        }

        void LoadAndRenderChunks()
        {
            for (int i = 0; i < 4; i++)
            {
                if (buildList.Count != 0)
                {
                    BuildChunk(buildList[0]);
                    buildList.RemoveAt(0);
                }
            }

            for (int i = 0; i < updateList.Count; i++)
            {
                Chunk chunk = world.GetChunk(updateList[0].x, updateList[0].y, updateList[0].z);
                if (chunk != null)
                {
                    chunk.update = true;
                    updateList.RemoveAt(0);
                }
            }
        }

        void DeleteChunks()
        {
            if (timer == 10)
            {
                var chunksToDelete = new List<WorldPos>();
                foreach (var chunk in world.chunks)
                {
                    float distance = Vector3.Distance(new Vector3(chunk.Value.pos.x, 0, chunk.Value.pos.z),
                        new Vector3(transform.position.x, 0, transform.position.z));
                    if (distance > 256)
                    {
                        chunksToDelete.Add(chunk.Key);
                    }
                }

                foreach (var chunk in chunksToDelete)
                {
                    world.DestroyChunk(chunk.x, chunk.y, chunk.z);
                }

                timer = 0;
            }

            timer++;
        }
        
        private void Update()
        {
            DeleteChunks();
            FindChunksToLoad();
            LoadAndRenderChunks();
        }
    }
}