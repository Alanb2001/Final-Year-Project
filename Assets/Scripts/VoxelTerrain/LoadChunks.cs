using System.Collections.Generic;
using UnityEngine;

namespace VoxelTerrain
{
    public class LoadChunks : MonoBehaviour
    {
        public World world;
        private List<WorldPos> updateList = new List<WorldPos>();
        private List<WorldPos> buildList = new List<WorldPos>();
        private Vector3Int[] chunkPositions;

        private int timer = 0;

        [SerializeField, Range(2, 10)] private int perimeter = 7;

        private void Awake()
        {
            CreatChunkPositions();
        }

        private void CreatChunkPositions()
        {
            chunkPositions = new Vector3Int[(int)Mathf.Pow((float)perimeter * 2.0f, 3.0f)];
            chunkPositions[0] = Vector3Int.zero;

            uint i = 1;
            for (int x = -1; x >= -perimeter; x--)
            {
                for (int y = -1; y >= -perimeter; y--)
                {
                    for (int z = -1; z >= -perimeter; z--)
                    {
                        for (int X = -1; X <= 1; X += 2)
                        {
                            print(X);
                            for (int Y = -1; Y <= 1; Y += 2)
                            {
                                for (int Z = -1; Z <= 1; Z += 2)
                                {
                                    chunkPositions[i] = new Vector3Int(x * X, y * Y, z * Z);
                                    i++;
                                }
                            }
                        }
                    }
                }
            }
        }

        void FindChunksToLoad()
        {
            // Get the position of this gameobject to generate around
            WorldPos playerPos = new WorldPos(
                Mathf.FloorToInt(transform.position.x / Chunk.chunkSize) * Chunk.chunkSize,
                Mathf.FloorToInt(transform.position.y / Chunk.chunkSize) * Chunk.chunkSize,
                Mathf.FloorToInt(transform.position.z / Chunk.chunkSize) * Chunk.chunkSize);

            // If there aren't already chunks to generate
            if (updateList.Count == 0)
            {
                // Cycle through the array of positions
                for (int i = 0; i < chunkPositions.Length; i++)
                {
                    // Translate the player position and array position into chunk position
                    WorldPos newChunkPos = new WorldPos(chunkPositions[i].x * Chunk.chunkSize + playerPos.x,
                        chunkPositions[i].y * Chunk.chunkSize + playerPos.y,
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
                    for (int x = newChunkPos.x - Chunk.chunkSize;
                        x <= newChunkPos.x + Chunk.chunkSize;
                        x += Chunk.chunkSize)
                    {
                        for (int y = newChunkPos.y - Chunk.chunkSize;
                            y <= newChunkPos.y + Chunk.chunkSize;
                            y += Chunk.chunkSize)
                        {
                            for (int z = newChunkPos.z - Chunk.chunkSize;
                                z <= newChunkPos.z + Chunk.chunkSize;
                                z += Chunk.chunkSize)
                            {
                                buildList.Add(new WorldPos(x, y, z));
                            }
                        }

                        updateList.Add(new WorldPos(newChunkPos.x, newChunkPos.y, newChunkPos.z));
                    }

                    return;
                }
            }
        }

        void BuildChunk(WorldPos pos)
        {
            if (world.GetChunk(pos.x, pos.y, pos.z) == null)
            {
                world.CreateChunk(pos.x, pos.y, pos.z);
            }
        }

        void LoadAndRenderChunks()
        {
            if (buildList.Count != 0)
            {
                for (int i = 0; i < buildList.Count && i < 8; i++)
                {
                    BuildChunk(buildList[0]);
                    buildList.RemoveAt(0);
                }

                // If chunks were built return early
                return;
            }

            if (updateList.Count != 0)
            {
                Chunk chunk = world.GetChunk(updateList[0].x, updateList[0].y, updateList[0].z);
                if (chunk != null)
                {
                    chunk.update = true;
                }

                updateList.RemoveAt(0);
            }
        }

        bool DeleteChunks()
        {
            if (timer == 10)
            {
                var chunksToDelete = new List<WorldPos>();
                foreach (var chunk in world.chunks)
                {
                    float distance = Vector3.Distance(
                        new Vector3(chunk.Value.pos.x, chunk.Value.pos.y, chunk.Value.pos.z),
                        new Vector3(transform.position.x, transform.position.y, transform.position.z));
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
                return true;
            }

            timer++;
            return false;
        }

        private void Update()
        {
            if (DeleteChunks())
            {
                return;
            }

            FindChunksToLoad();
            LoadAndRenderChunks();
        }
    }
}