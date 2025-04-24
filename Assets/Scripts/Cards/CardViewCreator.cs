using System;
using DG.Tweening;
using UnityEngine;

public class CardViewCreator : Singleton<CardViewCreator>
{
    [SerializeField] private CardView cardViewPrefab;

    public CardView CreateCardView(CardModel cardModel, Vector3 position, Quaternion rotation, float duration)
    {
        CardView cardView = Instantiate(cardViewPrefab, position, rotation);
        cardView.transform.localScale = Vector3.zero;
        cardView.transform.DOScale(Vector3.one, duration);
        cardView.Setup(cardModel);
        return cardView;
    }
}
