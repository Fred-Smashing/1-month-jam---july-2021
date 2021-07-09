using System.Collections.Generic;
using UnityEngine;
using Utility.TriangulatorUtil;

namespace Utility.Meshes
{
    [ExecuteAlways]
    public class GenerateMesh
    {
        private List<Vector2> m_points = new List<Vector2>();
        private MeshFilter m_filter;

        public GenerateMesh(Vector2[] points, MeshFilter meshFilter)
        {
            m_points = new List<Vector2>(points);
            m_filter = meshFilter;

            Init();
            UpdateMesh();
        }

        Mesh mesh;
        private void Init()
        {
            mesh = new Mesh();
            m_filter.mesh = mesh;
        }

        private void UpdateMesh()
        {
            Triangulator tr = new Triangulator(m_points.ToArray());
            int[] indices = tr.Triangulate();

            Vector3[] vertices = new Vector3[m_points.ToArray().Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector3(m_points[i].x, m_points[i].y, 0);
            }

            mesh.vertices = vertices;
            mesh.triangles = indices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }
    }
}
