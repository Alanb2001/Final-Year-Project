using UnityEngine;
using System.Collections.Generic;

namespace VoxelTerrain
{
    public class World : MonoBehaviour
    {
        public readonly Dictionary<WorldPos, Chunk> chunks = new Dictionary<WorldPos, Chunk>();
        public GameObject chunkPrefab;
        public string worldName = "world";

        public void CreateChunk(int x, int y, int z)
        {
            // The coordinates of this chunk in the world
            var worldPos = new WorldPos(x, y, z);
            
            // Instantiate the chunk at the coordinates using the chunk prefab
            var newChunkObject = Instantiate(chunkPrefab, new Vector3(worldPos.x, worldPos.y, worldPos.z),
                Quaternion.Euler(Vector3.zero));
            
            // Get the object's chunk component
            var newChunk = newChunkObject.GetComponent<Chunk>();

            // Assign its values
            newChunk.pos = worldPos;
            newChunk.world = this;
            
            // Add it to the chunks dictionary with the position as the key
            chunks.Add(worldPos, newChunk);

            var terrainGen = newChunkObject.GetComponent<TerrainGen>();
            newChunk = terrainGen.ChunkGen(newChunk);
            newChunk.SetBlocksUnmodified();
            Serialisation.Load(newChunk);
        }

        public void DestroyChunk(int x, int y, int z)
        {
            if (!chunks.TryGetValue(new WorldPos(x, y, z), out var chunk)) return;
            
            Serialisation.SaveChunk(chunk);
            Destroy(chunk.gameObject);
            chunks.Remove(new WorldPos(x, y, z));
        }
        
        public Chunk GetChunk(int x, int y, int z)
        {
            var pos = new WorldPos();
            float multiple = Chunk.ChunkSize;
            pos.x = Mathf.FloorToInt(x / multiple) * Chunk.ChunkSize;
            pos.y = Mathf.FloorToInt(y / multiple) * Chunk.ChunkSize;
            pos.z = Mathf.FloorToInt(z / multiple) * Chunk.ChunkSize;
            chunks.TryGetValue(pos, out var containerChunk);

            return containerChunk;
        }

        public Block GetBlock(int x, int y, int z)
        {
            var containerChunk = GetChunk(x, y, z);
            if (containerChunk == null) return new BlockAir();
            
            var block = containerChunk.GetBlock(x - containerChunk.pos.x, y - containerChunk.pos.y,
                z - containerChunk.pos.z);

            return block;
        }

        public void SetBlock(int x, int y, int z, Block block)
        {
            var chunk = GetChunk(x, y, z);

            if (chunk == null) return;
            
            chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, block);
            chunk.update = true;
                
            UpdateIfEqual(x - chunk.pos.x, 0, new WorldPos(x - 1, y, z));
            UpdateIfEqual(x - chunk.pos.x, Chunk.ChunkSize - 1, new WorldPos(x + 1, y, z));
            UpdateIfEqual(y - chunk.pos.y, 0, new WorldPos(x, y - 1, z));
            UpdateIfEqual(y - chunk.pos.y, Chunk.ChunkSize - 1, new WorldPos(x, y + 1, z));
            UpdateIfEqual(z - chunk.pos.z, 0, new WorldPos(x, y, z - 1));
            UpdateIfEqual(z - chunk.pos.z, Chunk.ChunkSize - 1, new WorldPos(x, y, z + 1));
        }

        private void UpdateIfEqual(int value1, int value2, WorldPos pos)
        {
            if (value1 != value2) return;
            
            var chunk = GetChunk(pos.x, pos.y, pos.z);
            if (chunk != null)
            {
                chunk.update = true;
            }
        }
    }
}
