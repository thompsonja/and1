using UnityEngine;
using DG.Tweening;


[ExecuteInEditMode]
public class HexHoverHandler : MonoBehaviour
{
    public float scaleFactor = 5f;
    private bool hasInitialized = false;
    private float initialScaleY;

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
            initialScaleY = transform.localScale[1];
            hasInitialized = true;
        }
        transform.DOScaleY(status ? initialScaleY * scaleFactor : initialScaleY, 1);
        // transform.DOScale(status ? initialScale * scaleFactor : initialScale, 1);
        // transform.localScale = status ? initialScale * scaleFactor : initialScale;
    }
}