using UnityEngine;

namespace VoxelTerrain
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class Chunk : MonoBehaviour
    {
        public readonly Block[, ,] blocks = new Block[ChunkSize, ChunkSize, ChunkSize];
        public const int ChunkSize = 16;
        public bool update;
        public World world;
        public WorldPos pos;
        public bool rendered;
        
        private MeshFilter _filter;
        private MeshCollider _coll;
        
        // Use this for initialisation.
        private void Start()
        {
            _filter = gameObject.GetComponent<MeshFilter>();
            _coll = gameObject.GetComponent<MeshCollider>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (!update) return;
            
            update = false;
            UpdateChunk();
        }

        public void SetBlocksUnmodified()
        {
            foreach (var block in blocks)
            {
                block.changed = false;
            }
        }
        
        public Block GetBlock(int x, int y, int z)
        {
            if (InRange(x) && InRange(y) && InRange(z))
            {
                return blocks[x, y, z];
            }
            return world.GetBlock(pos.x + x, pos.y + y, pos.z + z);
        }

        public static bool InRange(int index)
        {
            return index >= 0 && index < ChunkSize;
        }

        public void SetBlock(int x, int y, int z, Block block)
        {
            if (InRange(x) && InRange(y) && InRange(z))
            {
                blocks[x, y, z] = block;
            }
            else
            {
                world.SetBlock(pos.x + x, pos.y + y, pos.z + z, block);
            }
        }

        // Updates the chunk based on its contents
        private void UpdateChunk()
        {
            rendered = true;
            var meshData = new MeshData();
            for (var x = 0; x < ChunkSize; x++)
            {
                for (var y = 0; y < ChunkSize; y++)
                {
                    for (var z = 0; z < ChunkSize; z++)
                    {
                        meshData = blocks[x, y, z].BlockData(this, x, y, z, meshData);
                    }
                }
            }
            RenderMesh(meshData);
        }
        
        // Sends the calculated mesh information
        // to the mesh and collision components
        private void RenderMesh(MeshData meshData)
        {
            _filter.mesh.Clear();
            _filter.mesh.vertices = meshData.vertices.ToArray();
            _filter.mesh.triangles = meshData.triangles.ToArray();
            _filter.mesh.uv = meshData.uv.ToArray();
            _filter.mesh.RecalculateNormals();

            _coll.sharedMesh = null;
            
            var mesh = new Mesh
            {
                vertices = meshData.colVertices.ToArray(),
                triangles = meshData.colTriangles.ToArray()
            };
            
            mesh.RecalculateNormals();

            _coll.sharedMesh = mesh;
        }
    }
}
