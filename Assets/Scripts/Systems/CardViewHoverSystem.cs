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

    public void Show(CardModel card, Vector3 startPos, Vector3 endPos)
    {
        if (isHovering) return;
        isHovering = true;
        cardViewHover.gameObject.SetActive(true);
        cardViewHover.Setup(card);
        moveTween?.Kill();
        scaleTween?.Kill();
        cardViewHover.transform.position = startPos;
        moveTween = cardViewHover.transform.DOMove(endPos, moveDuration).SetEase(hoverEase);
        scaleTween = cardViewHover.transform.DOScale(Vector3.one * scaleUpFactor, moveDuration).SetEase(hoverEase);
        cardViewHover.transform.SetAsLastSibling();
    }

    public void Hide(CanvasGroup canvasGroup, Transform transform)
    {
        isHovering = false;
        moveTween?.Kill();
        scaleTween?.Kill();
        moveTween = cardViewHover.transform.DOMove(transform.position, moveDuration).SetEase(hoverEase);
        scaleTween = cardViewHover.transform.DOScale(Vector3.one, moveDuration).SetEase(hoverEase).OnComplete(() =>
        {
            if (canvasGroup)
            {
                canvasGroup.alpha = 1;
            }
            cardViewHover.gameObject.SetActive(false);
        });
    }
}
