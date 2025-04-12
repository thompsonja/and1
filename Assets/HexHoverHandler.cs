using UnityEngine;

[ExecuteInEditMode]
public class HexHoverHandler : MonoBehaviour
{
    public float scaleFactor = 1.1f;
    private bool hasInitialized = false;
    private Vector3 initialScale;

    private void OnMouseEnter()
    {
        UpdateScale(true);
    }

    private void OnMouseExit()
    {
        UpdateScale(false);
    }

    private void UpdateScale(bool status)
    {
        if (!hasInitialized)
        {
            initialScale = transform.localScale;
            hasInitialized = true;
        }
        transform.localScale = status ? initialScale * scaleFactor : initialScale;
    }
}