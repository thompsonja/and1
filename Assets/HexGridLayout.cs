using UnityEngine;

[ExecuteInEditMode]
public class HexGridLayout : MonoBehaviour
{
    [Header("Grid Settings")]
    public Vector2Int gridSize;

    [Header("Tile Settings")]
    public float outerSize = 1f;
    public float innerSize = 0f;
    public float height = 1f;
    public bool isFlatTopped;
    public Material material;

    public bool offsetHasExtraHex;

    // private bool shouldUpdateMesh = false;

    [ContextMenu("Regenerate Grid")]
    public void Regenerate()
    {
        LayoutGrid();
    }

    private void OnEnable()
    {
        LayoutGrid();
    }

    // private void OnValidate()
    // {
    //     if (Application.isPlaying)
    //     {
    //         shouldUpdateMesh = true;
    //     }
    // }

    // private void Update()
    // {
    //     if (shouldUpdateMesh)
    //     {
    //         LayoutGrid();
    //     }
    // }

    private void LayoutGrid()
    {
        // Remove old tiles
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        for (int y = 0; y < gridSize.y; y++)
        {

            for (int x = 0; x < gridSize.x; x++)
            {
                GameObject tile = new GameObject($"Hex {x},{y}", typeof(HexRenderer));

                tile.transform.SetParent(transform, false);
                tile.transform.localPosition = GetPositionForHexFromCoordinate(new Vector2Int(x, y));

                HexRenderer hexRenderer = tile.GetComponent<HexRenderer>();
                hexRenderer.isFlatTopped = isFlatTopped;
                hexRenderer.outerSize = outerSize;
                hexRenderer.innerSize = innerSize;
                hexRenderer.height = height;
                hexRenderer.material = material;
                hexRenderer.DrawMesh();

                MeshCollider collider = tile.AddComponent<MeshCollider>();
                collider.convex = true;
                // collider.sharedMesh = tile.GetComponent<MeshFilter>().mesh;
                tile.AddComponent<HexHoverHandler>();
            }
        }
    }

    public Vector3 GetPositionForHexFromCoordinate(Vector2Int coordinate)
    {
        int col = coordinate.x;
        int row = coordinate.y;

        float xPosition;
        float yPosition;
        float offset;
        float size = outerSize;
        float sizeDiff = outerSize - innerSize;

        if (!isFlatTopped)
        {
            bool shouldOffset = (row % 2) == 0;
            float width = Mathf.Sqrt(3) * size;
            float height = 2f * size;

            float horizontalDistance = width - sizeDiff;
            float verticalDistance = height * 3f / 4f - sizeDiff;

            offset = shouldOffset ? (width - sizeDiff) / 2 : 0;

            xPosition = col * horizontalDistance + offset;
            yPosition = row * verticalDistance;
        }
        else
        {
            bool shouldOffset = (col % 2) == 0;
            float width = 2f * size;
            float height = Mathf.Sqrt(3) * size;
            float horizontalDistance = width * 3f / 4f - sizeDiff;
            float verticalDistance = height - sizeDiff;

            offset = shouldOffset ? (height - sizeDiff) / 2f : 0;
            xPosition = col * horizontalDistance;
            yPosition = row * verticalDistance - offset;
        }

        return new Vector3(xPosition, 0, -yPosition);
    }
}