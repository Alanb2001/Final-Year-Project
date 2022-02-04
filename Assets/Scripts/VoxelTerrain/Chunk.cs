using UnityEngine;

namespace VoxelTerrain
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class Chunk : MonoBehaviour
    {
        private Block[,,] _blocks;
        public static int chunkSize = 16;
        public bool update = true;

        private MeshFilter _filter;
        private MeshCollider _coll;
        
        // Use this for initialisation.
        void Start()
        {
            _filter = gameObject.GetComponent<MeshFilter>();
            _coll = gameObject.GetComponent<MeshCollider>();
            
            // Past here is just to set up an example chunk
            _blocks = new Block[chunkSize, chunkSize, chunkSize];
            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    for (int z = 0; z < chunkSize; z++)
                    {
                        _blocks[x, y, z] = new BlockAir();
                    }
                }
            }
            _blocks[3, 5, 2] = new Block();
            _blocks[4, 5, 2] = new BlockGrass();
            UpdateChunk();
        }

        // Update is called once per frame
        void Update()
        {
        }

        public Block GetBlock(int x, int y, int z)
        {
            return _blocks[x, y, z];
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
