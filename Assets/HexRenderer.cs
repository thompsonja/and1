using System;
using System.Collections.Generic;
using UnityEngine;

public struct Face
{
    public List<Vector3> Vertices { get; private set; }
    public List<int> Triangles { get; private set; }
    public List<Vector2> Uvs { get; private set; }

    public Face(List<Vector3> vertices, List<int> triangles, List<Vector2> uvs)
    {
        Vertices = vertices;
        Triangles = triangles;
        Uvs = uvs;
    }
}


[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class HexRenderer : MonoBehaviour
{
    public Mesh mesh;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private List<Face> faces;

    private readonly float[] cosAngles = new float[6];
    private readonly float[] sinAngles = new float[6];
    private readonly float[] flatTopCosAngles = new float[6];
    private readonly float[] flatTopSinAngles = new float[6];

    public float innerSize;
    public float outerSize;
    public float height;
    public bool isFlatTopped;

    public Material material;

    public bool shouldUpdateMesh = false;

    private void EnsureComponents()
    {
        CacheAngles();

        meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null) meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null) meshRenderer = gameObject.AddComponent<MeshRenderer>();

        mesh = new Mesh
        {
            name = "Hex"
        };

        meshFilter.mesh = mesh;
        meshRenderer.material = material;
    }

    private void Awake()
    {
        EnsureComponents();
    }

    private void OnEnable()
    {
        EnsureComponents();
        DrawMesh();
    }

    public void OnValidate()
    {
        CacheAngles();

        if (Application.isPlaying)
        {
            shouldUpdateMesh = true;
        }
    }

    private void Update()
    {
        if (shouldUpdateMesh)
        {
            DrawMesh();
        }
    }

    public void DrawMesh()
    {
        DrawFaces();
        CombineFaces();
    }

    private void DrawFaces()
    {
        faces = new();

        // Top faces
        for (int point = 0; point < 6; point++)
        {
            faces.Add(CreateFace(innerSize, outerSize, height / 2f, height / 2f, point));
        }

        // Bottom faces
        for (int point = 0; point < 6; point++)
        {
            faces.Add(CreateFace(innerSize, outerSize, -height / 2f, -height / 2f, point, true));
        }

        // Outer faces
        for (int point = 0; point < 6; point++)
        {
            faces.Add(CreateFace(outerSize, outerSize, height / 2f, -height / 2f, point, true));
        }

        // Inner faces
        for (int point = 0; point < 6; point++)
        {
            faces.Add(CreateFace(innerSize, innerSize, height / 2f, -height / 2f, point));
        }
    }

    private void CombineFaces()
    {
        List<Vector3> vertices = new();
        List<int> tris = new();
        List<Vector2> uvs = new();

        // Add vertices
        for (int i = 0; i < faces.Count; i++)
        {
            vertices.AddRange(faces[i].Vertices);
            uvs.AddRange(faces[i].Uvs);

            // Create offset triangles
            int offset = 4 * i;
            foreach (int triangle in faces[i].Triangles)
            {
                tris.Add(triangle + offset);
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
    }

    private Face CreateFace(float innerRad, float outerRad, float heightA, float heightB, int point, bool reverse = false)
    {
        Vector3 pointA = GetPoint(innerRad, heightB, point);
        Vector3 pointB = GetPoint(innerRad, heightB, point < 5 ? point + 1 : 0); // Add one to point unless last point, then use 0
        Vector3 pointC = GetPoint(outerRad, heightA, point < 5 ? point + 1 : 0);
        Vector3 pointD = GetPoint(outerRad, heightA, point);

        List<Vector3> vertices = new() { pointA, pointB, pointC, pointD };
        List<int> triangles = new() { 0, 1, 2, 2, 3, 0 };
        List<Vector2> uvs = new() { new(0, 0), new(1, 0), new(1, 1), new(0, 1) };

        if (reverse)
        {
            vertices.Reverse();
        }
        return new Face(vertices, triangles, uvs);
    }

    protected Vector3 GetPoint(float size, float height, int index)
    {
        Tuple<float, float> angles = new(
            isFlatTopped ? flatTopCosAngles[index % 6] : cosAngles[index % 6],
            isFlatTopped ? flatTopSinAngles[index % 6] : sinAngles[index % 6]);
        return new Vector3(size * angles.Item1, height, size * angles.Item2);
    }

    private void CacheAngles()
    {
        for (int i = 0; i < 6; i++)
        {
            float angle = 60f * i * Mathf.Deg2Rad;
            flatTopCosAngles[i] = Mathf.Cos(angle);
            flatTopSinAngles[i] = Mathf.Sin(angle);

            float nonFlatTopAngle = (60f * i - 30) * Mathf.Deg2Rad;
            cosAngles[i] = Mathf.Cos(nonFlatTopAngle);
            sinAngles[i] = Mathf.Sin(nonFlatTopAngle);
        }
    }
}
