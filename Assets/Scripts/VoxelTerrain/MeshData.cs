using System.Collections.Generic;
using UnityEngine;

namespace VoxelTerrain
{
    public class MeshData
    {
        public readonly List<Vector3> vertices = new List<Vector3>();
        public readonly List<int> triangles = new List<int>();
        public readonly List<Vector2> uv = new List<Vector2>();
        public readonly List<Vector3> colVertices = new List<Vector3>();
        public readonly List<int> colTriangles = new List<int>();
        public bool useRenderDataForCol;

        public void AddQuadTriangles()
        {
            triangles.Add(vertices.Count - 4);
            triangles.Add(vertices.Count - 3);
            triangles.Add(vertices.Count - 2);
            triangles.Add(vertices.Count - 4);
            triangles.Add(vertices.Count - 2);
            triangles.Add(vertices.Count - 1);
            
            if (!useRenderDataForCol) return;
            
            colTriangles.Add(vertices.Count - 4);
            colTriangles.Add(vertices.Count - 3);
            colTriangles.Add(vertices.Count - 2);
            colTriangles.Add(vertices.Count - 4);
            colTriangles.Add(vertices.Count - 2);
            colTriangles.Add(vertices.Count - 1);
        }

        public void AddVertex(Vector3 vertex)
        {
            vertices.Add(vertex);
            if (useRenderDataForCol)
            {
                colVertices.Add(vertex);
            }
        }
    }
}
