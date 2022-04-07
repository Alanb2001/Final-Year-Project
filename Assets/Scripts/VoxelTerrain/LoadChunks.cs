using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace VoxelTerrain
{
    public class LoadChunks : MonoBehaviour
    {
       //[BurstCompile]
       //public struct GenChunk : IJob
       //{
       //    public NativeList<WorldPos> chunkPositions;

       //    public float3 position;
       //    
       //    public World world;
       //    private readonly List<WorldPos> _updateList;
       //    private readonly List<WorldPos> _buildList;
       //    private WorldPos[] _chunkPositions;

       //    private int _timer;

       //    public int perimeter;


       //    public void Execute()
       //    {
       //        //for (int i = 0; i < chunkPositions.Length; i++)
       //        //{
       //        //    var worldPos = chunkPositions[i];
       //        //    var per = perimeter;
       //        //}
       //    }

       //    private void Awake()
       //    {
       //        CreatChunkPositions();
       //    }

       //    private void CreatChunkPositions()
       //    {
       //        _chunkPositions = new WorldPos[(int)Mathf.Pow(perimeter * 2, 3)];

       //        var i = 0;
       //        for (var x = 0; x < perimeter; x++)
       //        {
       //            for (var y = 0; y < perimeter; y++)
       //            {
       //                for (var z = 0; z < perimeter; z++)
       //                {
       //                    if (x == 0 && y == 0 && z == 0)
       //                    {
       //                        _chunkPositions[i++] = new WorldPos(x, y, z);
       //                        continue;
       //                    }

       //                    _chunkPositions[i++] = new WorldPos(x, y, z);
       //                    _chunkPositions[i++] = new WorldPos(x * -1, y * -1, z * -1);
       //                    _chunkPositions[i++] = new WorldPos(x * -1, y, z);
       //                    _chunkPositions[i++] = new WorldPos(x, y * -1, z * -1);
       //                    _chunkPositions[i++] = new WorldPos(x, y * -1, z);
       //                    _chunkPositions[i++] = new WorldPos(x * -1, y, z * -1);
       //                    _chunkPositions[i++] = new WorldPos(x, y, z * -1);
       //                    _chunkPositions[i++] = new WorldPos(x * -1, y * -1, z);
       //                }
       //            }
       //        }
       //    }

       //    private void FindChunksToLoad()
       //    {
       //        // Get the position of this gameObject to generate around
       //        WorldPos playerPos = new WorldPos(
       //            Mathf.FloorToInt(position.x / Chunk.ChunkSize) * Chunk.ChunkSize,
       //            Mathf.FloorToInt(position.y / Chunk.ChunkSize) * Chunk.ChunkSize,
       //            Mathf.FloorToInt(position.z / Chunk.ChunkSize) * Chunk.ChunkSize);

       //        // If there aren't already chunks to generate
       //        if (_updateList.Count != 0) return;

       //        // Cycle through the array of positions
       //        for (var i = 0; i < _chunkPositions.Length; i++)
       //        {
       //            // Translate the player position and array position into chunk position
       //            var newChunkPos = new WorldPos(
       //                _chunkPositions[i].x * Chunk.ChunkSize + playerPos.x,
       //                _chunkPositions[i].y * Chunk.ChunkSize + playerPos.y,
       //                _chunkPositions[i].z * Chunk.ChunkSize + playerPos.z);

       //            // Get the chunk in the defined position
       //            Chunk newChunk = world.GetChunk(newChunkPos.x, newChunkPos.y, newChunkPos.z);

       //            // If the chunk already exists and it's already
       //            // rendered or in queue to be rendered continue
       //            if (newChunk != null && (newChunk.rendered || _updateList.Contains(newChunkPos)))
       //            {
       //                continue;
       //            }

       //            // Load a column of chunks in this position
       //            for (var x = newChunkPos.x - Chunk.ChunkSize;
       //                x <= newChunkPos.x + Chunk.ChunkSize;
       //                x += Chunk.ChunkSize)
       //            {
       //                for (var y = newChunkPos.y - Chunk.ChunkSize;
       //                    y <= newChunkPos.y + Chunk.ChunkSize;
       //                    y += Chunk.ChunkSize)
       //                {
       //                    for (var z = newChunkPos.z - Chunk.ChunkSize;
       //                        z <= newChunkPos.z + Chunk.ChunkSize;
       //                        z += Chunk.ChunkSize)
       //                    {
       //                        _buildList.Add(new WorldPos(x, y, z));
       //                    }
       //                }

       //                _updateList.Add(new WorldPos(newChunkPos.x, newChunkPos.y, newChunkPos.z));
       //            }

       //            return;
       //        }
       //    }

       //    private void BuildChunk(WorldPos pos)
       //    {
       //        if (world.GetChunk(pos.x, pos.y, pos.z) == null)
       //        {
       //            world.CreateChunk(pos.x, pos.y, pos.z);
       //        }
       //    }

       //    private void LoadAndRenderChunks()
       //    {
       //        if (_buildList.Count != 0)
       //        {
       //            for (int i = 0; i < _buildList.Count && i < 8; i++)
       //            {
       //                BuildChunk(_buildList[0]);
       //                _buildList.RemoveAt(0);
       //            }

       //            // If chunks were built return early
       //            return;
       //        }

       //        if (_updateList.Count == 0) return;

       //        var chunk = world.GetChunk(_updateList[0].x, _updateList[0].y, _updateList[0].z);
       //        if (chunk != null)
       //        {
       //            chunk.update = true;
       //        }

       //        _updateList.RemoveAt(0);
       //    }

       //    private bool DeleteChunks()
       //    {
       //        if (_timer == 10)
       //        {
       //            var chunksToDelete = new List<WorldPos>();
       //            foreach (var chunk in world.chunks)
       //            {
       //                float distance = Vector3.Distance(
       //                    new Vector3(chunk.Value.pos.x, chunk.Value.pos.y, chunk.Value.pos.z),
       //                    new Vector3(position.x, position.y, position.z));
       //                if (distance > 256)
       //                {
       //                    chunksToDelete.Add(chunk.Key);
       //                }
       //            }

       //            foreach (var chunk in chunksToDelete)
       //            {
       //                world.DestroyChunk(chunk.x, chunk.y, chunk.z);
       //            }

       //            _timer = 0;
       //            return true;
       //        }

       //        _timer++;
       //        return false;
       //    }

       //    private void Update()
       //    {
       //        if (DeleteChunks())
       //        {
       //            return;
       //        }

       //        FindChunksToLoad();
       //        LoadAndRenderChunks();
       //    }
       //}

        public World world;
        private readonly List<WorldPos> _updateList = new List<WorldPos>();
        private readonly List<WorldPos> _buildList = new List<WorldPos>();
        private WorldPos[] _chunkPositions;

        private int _timer = 0;

        [SerializeField, Range(2, 10)] private int perimeter = 7;

        private void Awake()
        {
            CreatChunkPositions();
        }

        private void CreatChunkPositions()
        {
            _chunkPositions = new WorldPos[(int)Mathf.Pow(perimeter * 2, 3)];

            var i = 0;
            for (var x = 0; x < perimeter; x++)
            {
                for (var y = 0; y < perimeter; y++)
                {
                    for (var z = 0; z < perimeter; z++)
                    {
                        if (x == 0 && y == 0 && z == 0)
                        {
                            _chunkPositions[i++] = new WorldPos(x, y, z);
                            continue;
                        }

                        _chunkPositions[i++] = new WorldPos(x, y, z);
                        _chunkPositions[i++] = new WorldPos(x * -1, y * -1, z * -1);
                        _chunkPositions[i++] = new WorldPos(x * -1, y, z);
                        _chunkPositions[i++] = new WorldPos(x, y * -1, z * -1);
                        _chunkPositions[i++] = new WorldPos(x, y * -1, z);
                        _chunkPositions[i++] = new WorldPos(x * -1, y, z * -1);
                        _chunkPositions[i++] = new WorldPos(x, y, z * -1);
                        _chunkPositions[i++] = new WorldPos(x * -1, y * -1, z);
                    }
                }
            }
        }

        private void FindChunksToLoad()
        {
            // Get the position of this gameObject to generate around
            WorldPos playerPos = new WorldPos(
                Mathf.FloorToInt(transform.position.x / Chunk.ChunkSize) * Chunk.ChunkSize,
                Mathf.FloorToInt(transform.position.y / Chunk.ChunkSize) * Chunk.ChunkSize,
                Mathf.FloorToInt(transform.position.z / Chunk.ChunkSize) * Chunk.ChunkSize);

            // If there aren't already chunks to generate
            if (_updateList.Count != 0) return;

            // Cycle through the array of positions
            for (var i = 0; i < _chunkPositions.Length; i++)
            {
                // Translate the player position and array position into chunk position
                var newChunkPos = new WorldPos(
                    _chunkPositions[i].x * Chunk.ChunkSize + playerPos.x,
                    _chunkPositions[i].y * Chunk.ChunkSize + playerPos.y,
                    _chunkPositions[i].z * Chunk.ChunkSize + playerPos.z);

                // Get the chunk in the defined position
                Chunk newChunk = world.GetChunk(newChunkPos.x, newChunkPos.y, newChunkPos.z);

                // If the chunk already exists and it's already
                // rendered or in queue to be rendered continue
                if (newChunk != null && (newChunk.rendered || _updateList.Contains(newChunkPos)))
                {
                    continue;
                }

                // Load a column of chunks in this position
                for (var x = newChunkPos.x - Chunk.ChunkSize;
                    x <= newChunkPos.x + Chunk.ChunkSize;
                    x += Chunk.ChunkSize)
                {
                    for (var y = newChunkPos.y - Chunk.ChunkSize;
                        y <= newChunkPos.y + Chunk.ChunkSize;
                        y += Chunk.ChunkSize)
                    {
                        for (var z = newChunkPos.z - Chunk.ChunkSize;
                            z <= newChunkPos.z + Chunk.ChunkSize;
                            z += Chunk.ChunkSize)
                        {
                            _buildList.Add(new WorldPos(x, y, z));
                        }
                    }

                    _updateList.Add(new WorldPos(newChunkPos.x, newChunkPos.y, newChunkPos.z));
                }

                return;
            }
        }

        private void BuildChunk(WorldPos pos)
        {
            if (world.GetChunk(pos.x, pos.y, pos.z) == null)
            {
                world.CreateChunk(pos.x, pos.y, pos.z);
            }
        }

        private void LoadAndRenderChunks()
        {
            if (_buildList.Count != 0)
            {
                for (int i = 0; i < _buildList.Count && i < 8; i++)
                {
                    BuildChunk(_buildList[0]);
                    _buildList.RemoveAt(0);
                }

                // If chunks were built return early
                return;
            }

            if (_updateList.Count == 0) return;

            var chunk = world.GetChunk(_updateList[0].x, _updateList[0].y, _updateList[0].z);
            if (chunk != null)
            {
                chunk.update = true;
            }

            _updateList.RemoveAt(0);
        }

        private bool DeleteChunks()
        {
            if (_timer == 10)
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

                _timer = 0;
                return true;
            }

            _timer++;
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