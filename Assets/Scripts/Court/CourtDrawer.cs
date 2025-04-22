using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CourtDrawer : MonoBehaviour
{
    public Material lineMaterial;
    public float lineWidth = 0.05f;

    // width is z-axis
    private float courtWidth = 14.326f;  // 94ft in m, but halfcourt (divide by 2)

    // length is x-axis
    private float courtLength = 15.24f; // 50ft in m
    private float paintWidth = 4.877f;  // 16 ft in m
    private float paintLength = 5.791f;  // 19 ft in m
    private float hoopRadius = 0.457f; // 1.5 ft in m
    private float backboardToCenterOfHoopDistance = 0.381f; // 15 in in m
    private float sidelineDist = 6.706f;  // 22ft 3pt line to sidelines
    private float sidelineWidth = 4.267f;  // 14ft from baseline
    private float distanceToHoop = 1.219f;  // 4ft from baseline
    private float threePointLineMaxDistance = 7.239f;  // 23.75ft
    private int arcSegments = 60;

    [ContextMenu("Regenerate Court")]
    public void Regenerate()
    {
        DrawCourt();
    }

    private void OnEnable()
    {
        DrawCourt();
    }

    private void DrawCourt()
    {
        ClearOldLines();

        DrawRectangle(new Vector2(0, 0), courtWidth, courtLength, "court");
        DrawRectangle(new Vector2((paintLength - courtLength) / 2, 0), paintWidth, paintLength, "paint");
        DrawThreePointLine(new Vector2(-sidelineWidth - distanceToHoop, 0), arcSegments);
        DrawHalfArc(new Vector2((hoopRadius - courtLength + distanceToHoop) / 2, 0), hoopRadius, arcSegments, "hoop");
        // DrawHalfArc(new Vector2(0, 0), threePointRadius, arcSegments);
    }

    private void DrawRectangle(Vector2 center, float width, float length, string name)
    {
        Vector3[] corners = new Vector3[5];
        corners[0] = new Vector3(center.x - length / 2, 0.1f, center.y - width / 2);
        corners[1] = new Vector3(center.x + length / 2, 0.1f, center.y - width / 2);
        corners[2] = new Vector3(center.x + length / 2, 0.1f, center.y + width / 2);
        corners[3] = new Vector3(center.x - length / 2, 0.1f, center.y + width / 2);
        corners[4] = new Vector3(center.x - length / 2, 0.1f, center.y - width / 2);

        CreateLine(corners, name);
    }

    private void DrawThreePointLine(Vector2 center, int segments)
    {
        float arcStartAngle = Mathf.Atan(sidelineDist / (sidelineWidth - backboardToCenterOfHoopDistance - distanceToHoop));

        Vector3[] arcPoints = new Vector3[segments + 1];
        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)arcSegments;
            float angle = Mathf.Lerp(-arcStartAngle, arcStartAngle, t);
            float x = center.x + Mathf.Cos(angle) * threePointLineMaxDistance - distanceToHoop / 2;
            float z = center.y + Mathf.Sin(angle) * threePointLineMaxDistance;
            arcPoints[i] = new Vector3(x, 0.01f, z);
        }

        CreateLine(arcPoints, "three_point_arc");

        Vector3 topStart = new Vector3(center.x - sidelineWidth / 2, 0.01f, center.y - sidelineDist);
        Vector3 topEnd = new Vector3(center.x + sidelineWidth / 2, 0.01f, center.y - sidelineDist);
        CreateLine(new Vector3[] { topStart, topEnd }, "three_point_top");

        Vector3 bottomStart = new Vector3(center.x - sidelineWidth / 2, 0.01f, center.y + sidelineDist);
        Vector3 bottomEnd = new Vector3(center.x + sidelineWidth / 2, 0.01f, center.y + sidelineDist);
        CreateLine(new Vector3[] { bottomStart, bottomEnd }, "three_point_bottom");
    }

    private void DrawHalfArc(Vector2 center, float radius, int segments, string name)
    {
        Vector3[] points = new Vector3[segments + 1];
        for (int i = 0; i <= segments; i++)
        {
            float angle = Mathf.Lerp(-Mathf.PI / 2f, Mathf.PI / 2f, i / (float)segments);
            float x = center.x + Mathf.Cos(angle) * radius;
            float z = center.y + Mathf.Sin(angle) * radius;
            points[i] = new Vector3(x, 0.01f, z);
        }

        CreateLine(points, name);
    }

    private void CreateLine(Vector3[] points, string name)
    {
        GameObject line = new GameObject($"CourtLine_{name}");
        line.transform.parent = transform;
        LineRenderer lr = line.AddComponent<LineRenderer>();
        lr.useWorldSpace = false;
        lr.positionCount = points.Length;
        lr.SetPositions(points);
        lr.material = lineMaterial;
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        lr.loop = false;
    }

    private void ClearOldLines()
    {
        List<GameObject> toDestroy = new();
        foreach (Transform child in transform)
        {
            if (child.name.StartsWith("CourtLine"))
            {
                toDestroy.Add(child.gameObject);
            }
        }

        foreach (GameObject obj in toDestroy)
        {
            if (!Application.isPlaying)
            {
                DestroyImmediate(obj);
            }
            else
            {
                Destroy(obj);
            }
        }
    }

}
