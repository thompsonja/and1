using UnityEngine;

public static class MouseUtil
{
    private static Camera camera = Camera.main;
    public static Vector3 GetMousePositionInWorldSpace(float zValue = 0f)
    {
        Plane dragPlane = new(camera.transform.forward, new Vector3(0, 0, zValue));
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (dragPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }

        // This shouldn't happen
        return Vector3.zero;
    }


    public static Vector2 GetMousePositionInRect(RectTransform parent)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, Input.mousePosition, null, out Vector2 localPoint);
        return localPoint;
    }
}
