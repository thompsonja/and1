using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;

public class HandView : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;
    private readonly List<CardView> cards = new();
    public int maxCards = 10;
    public float duration = 0.15f;

    private void Start()
    {
        GameSystem.Instance.AddListener<PlayerController>(GameSystem.GameEvent.PlayerSelectedChanged, OnPlayerSelectedChanged);
    }

    private void OnDestroy()
    {
        if (GameSystem.Instance)
        {
            GameSystem.Instance.RemoveListener<PlayerController>(GameSystem.GameEvent.PlayerSelectedChanged, OnPlayerSelectedChanged);
        }
    }

    private void OnPlayerSelectedChanged(PlayerController player)
    {
        // Clear the current hand when player changes
        ClearHand();
    }

    public IEnumerator AddCard(CardView cardView)
    {
        cardView.transform.SetParent(transform, true);
        cards.Add(cardView);
        yield return UpdateCardPositions();
    }

    public CardView RemoveCard(CardModel card)
    {
        CardView cardView = GetCardView(card);
        if (cardView == null) return null;
        cards.Remove(cardView);
        StartCoroutine(UpdateCardPositions());
        return cardView;
    }

    private CardView GetCardView(CardModel card)
    {
        return cards.Where(c => c.CardModel == card).FirstOrDefault();
    }

    private IEnumerator UpdateCardPositions()
    {
        if (cards.Count == 0) yield break;
        float cardSpacing = 1f / maxCards;
        float firstCardPosition = 0.5f - (cards.Count - 1) * cardSpacing / 2;
        Spline spline = splineContainer.Spline;
        for (int i = 0; i < cards.Count; i++)
        {
            float pos = firstCardPosition + i * cardSpacing;
            Vector3 splinePosition = spline.EvaluatePosition(pos);
            Vector3 forward = spline.EvaluateTangent(pos);
            Vector3 up = spline.EvaluateUpVector(pos);
            Quaternion rotation = Quaternion.LookRotation(-up, Vector3.Cross(-up, forward).normalized);
            cards[i].transform.DOMove(splinePosition + transform.position + 0.01f * i * Vector3.back, duration);
            cards[i].transform.DORotate(rotation.eulerAngles, duration);
        }
        yield return new WaitForSeconds(duration);
    }

    private void ClearHand()
    {
        foreach (var card in cards)
        {
            Destroy(card.gameObject);
        }
        cards.Clear();
    }
}
