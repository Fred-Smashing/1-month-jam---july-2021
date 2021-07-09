using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerSettingsSO playerSettings;

    private PolygonCollider2D _collider;
    private MeshFilter _meshFilter;

    private Utility.Meshes.GenerateMesh _meshGenerator;

    private void Awake()
    {
        _collider = GetComponent<PolygonCollider2D>();
        _meshFilter = GetComponent<MeshFilter>();

        _meshGenerator = new Utility.Meshes.GenerateMesh(_collider.points, _meshFilter);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            IncreaseSizeOfMesh();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            ReduceSizeOfMesh();
        }
    }

    private void IncreaseSizeOfMesh()
    {
        Vector2[] points = _collider.points;

        for (int i = 0; i < points.Length; i++)
        {
            var dirToCenter = (points[i] - (Vector2)_collider.bounds.center).normalized;

            points[i] += new Vector2(Random.Range(0.01f, 0.2f) * dirToCenter.x, Random.Range(0.01f, 0.2f) * dirToCenter.y);
        }

        _collider.SetPath(0, points);

        Debug.Log("Increase Mesh Size");
    }

    private void ReduceSizeOfMesh()
    {
        Vector2[] points = _collider.points;

        for (int i = 0; i < points.Length; i++)
        {
            var dirToCenter = (points[i] - (Vector2)_collider.bounds.center).normalized;

            points[i] -= new Vector2(Random.Range(0.01f, 0.2f) * dirToCenter.x, Random.Range(0.01f, 0.2f) * dirToCenter.y);
        }

        _collider.SetPath(0, points);

        Debug.Log("Reduce Mesh Size");
    }

    private void LateUpdate()
    {
        _meshGenerator = new Utility.Meshes.GenerateMesh(_collider.points, _meshFilter);
    }
}
