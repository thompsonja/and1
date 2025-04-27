using DG.Tweening;
using UnityEngine;

public class CardViewHoverSystem : Singleton<CardViewHoverSystem>
{
    [SerializeField] private CardView cardViewHover;
    [SerializeField] private float moveDuration = 0.15f;
    [SerializeField] private float scaleUpFactor = 1.5f;
    private Ease hoverEase = Ease.OutQuad;

    private Tween moveTween;
    private Tween scaleTween;

    private bool isHovering = false;

    public void Show(CardModel card, Vector3 position)
    {
        if (isHovering) return;
        isHovering = true;
        cardViewHover.gameObject.SetActive(true);
        cardViewHover.Setup(card);
        moveTween?.Kill();
        scaleTween?.Kill();
        moveTween = cardViewHover.transform.DOMove(position, moveDuration).SetEase(hoverEase);
        scaleTween = cardViewHover.transform.DOScale(Vector3.one * scaleUpFactor, moveDuration);
    }

    public void Hide(Vector3 position)
    {
        isHovering = false;
        moveTween?.Kill();
        scaleTween?.Kill();
        moveTween = cardViewHover.transform.DOMove(position, moveDuration).SetEase(hoverEase);
        scaleTween = cardViewHover.transform.DOScale(Vector3.one, moveDuration).SetEase(hoverEase);
        cardViewHover.gameObject.SetActive(false);
    }
}
