using UnityEngine;

namespace VoxelTerrain
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class Chunk : MonoBehaviour
    {
        public Block[, ,] _blocks = new Block[chunkSize, chunkSize, chunkSize];
        public static int chunkSize = 16;
        public bool update = true;
        public World world;
        public WorldPos pos;

        private MeshFilter _filter;
        private MeshCollider _coll;
        
        // Use this for initialisation.
        void Start()
        {
            _filter = gameObject.GetComponent<MeshFilter>();
            _coll = gameObject.GetComponent<MeshCollider>();
        }

        // Update is called once per frame
        void Update()
        {
            if (update)
            {
                update = false;
                UpdateChunk();
            }
        }

        public void SetBlocksUnmodified()
        {
            foreach (Block block in _blocks)
            {
                block.changed = false;
            }
        }
        
        public Block GetBlock(int x, int y, int z)
        {
            if (InRange(x) && InRange(y) && InRange(z))
            {
                return _blocks[x, y, z];
            }
            return world.GetBlock(pos.x + x, pos.y + y, pos.z + z);
        }

        public static bool InRange(int index)
        {
            if (index < 0 || index >= chunkSize)
            {
                return false;
            }
            return true;
        }

        public void SetBlock(int x, int y, int z, Block block)
        {
            if (InRange(x) && InRange(y) && InRange(z))
            {
                _blocks[x, y, z] = block;
            }
            else
            {
                world.SetBlock(pos.x + x, pos.y + y, pos.z + z, block);
            }
        }

        // Updates the chunk based on its contents
        void UpdateChunk()
        {
            MeshData meshData = new MeshData();
            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    for (int z = 0; z < chunkSize; z++)
                    {
                        meshData = _blocks[x, y, z].Blockdata(this, x, y, z, meshData);
                    }
                }
            }
            RenderMesh(meshData);
        }
        
        // Sends the calculated mesh information
        // to the mesh and collision components
        void RenderMesh(MeshData meshData)
        {
            _filter.mesh.Clear();
            _filter.mesh.vertices = meshData.vertices.ToArray();
            _filter.mesh.triangles = meshData.triangles.ToArray();
            _filter.mesh.uv = meshData.uv.ToArray();
            _filter.mesh.RecalculateNormals();

            _coll.sharedMesh = null;
            Mesh mesh = new Mesh();
            mesh.vertices = meshData.colVertices.ToArray();
            mesh.triangles = meshData.colTriangles.ToArray();
            mesh.RecalculateNormals();

            _coll.sharedMesh = mesh;
        }
    }
}
