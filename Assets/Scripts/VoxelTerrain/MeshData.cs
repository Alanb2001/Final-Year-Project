using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace VoxelTerrain
{
   //public struct CubeJob : IJob
   //{
   //    public struct MeshData
   //    {
   //        public NativeList<float3> Vertices { get; set; }
   //        public NativeList<int> Triangles { get; set; }
   //    }
   //
   //    public int3 position { get; set; }
   //    
   //    public MeshData meshData { get; set; }
   //    
   //    public void Execute()
   //    {
   //        for (int i = 0; i < 6; i++)
   //        {
   //            CreateFace((Block.Direction)i, position);
   //        }
   //    }
   //    
   //    private void CreateFace(Block.Direction direction, int3 pos)
   //    {
   //        var vertices = BlockExtensions.GetFaceVertices(direction, 1, pos);
   //            
   //        meshData.Vertices.AddRange(vertices);
   //
   //        vertices.Dispose();
   //        
   //        var vCount = meshData.Vertices.Length - 4;
   //
   //        meshData.Triangles.Add(vCount);
   //        meshData.Triangles.Add(vCount+ 1);
   //        meshData.Triangles.Add(vCount + 2);
   //        meshData.Triangles.Add(vCount);
   //        meshData.Triangles.Add(vCount + 2);
   //        meshData.Triangles.Add(vCount + 3);
   //    }
   //}
   //public class Cube : MonoBehaviour
   //{
   //    private MeshFilter _meshFilter;
   //    private JobHandle _jobHandle;
   //    private CubeJob.MeshData _meshData;

   //    private bool _jobCompleted;
   //    
   //    private void Awake()
   //    {
   //        _meshFilter = GetComponent<MeshFilter>();
   //    }
   //
   //    private void Start()
   //    {
   //        var meshData = new CubeJob.MeshData
   //        {
   //            Vertices = new NativeList<float3>(Allocator.TempJob),
   //            Triangles = new NativeList<int>(Allocator.TempJob)
   //        };
   //
   //        var cubeJob = new CubeJob()
   //        {
   //            position = int3.zero,
   //            meshData = meshData
   //        };
   //
   //        var jobHandle = cubeJob.Schedule();
   //    }
   //
   //    private void Update()
   //    {
   //        if (!_jobCompleted && _jobHandle.IsCompleted)
   //        {
   //            _jobCompleted = true;
   //            OnComplete();
   //        }
   //    }

   //    private void OnComplete()
   //    {
   //        _jobHandle.Complete();
   //        
   //        var mesh = new Mesh()
   //        {
   //            vertices = _meshData.Vertices.ToArray().Select(vertex => new Vector3(vertex.x, vertex.y, vertex.z)).ToArray(),
   //            triangles = _meshData.Triangles.ToArray()
   //        };
   //
   //        _meshData.Vertices.Dispose();
   //        _meshData.Triangles.Dispose();

   //        mesh.RecalculateNormals();
   //        mesh.RecalculateBounds();
   //        mesh.RecalculateTangents();
   //        
   //        _meshFilter.mesh = mesh;
   //    }
   //}
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
