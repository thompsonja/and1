using DG.Tweening;
using UnityEngine;

public class CardViewCreator : Singleton<CardViewCreator>
{
    [SerializeField] private CardView cardViewPrefab;

    public override void Init()
    {
        Debug.Log("CardViewCreator Init");
        if (cardViewPrefab == null)
        {
            Debug.LogError("CardViewCreator: cardViewPrefab is not assigned!");
        }
        base.Init();
    }

    public CardView CreateCardView(CardModel cardModel, Vector3 position, Quaternion rotation, float duration)
    {
        if (cardViewPrefab == null)
        {
            Debug.LogError("CardViewCreator: Cannot create card view - prefab is null!");
            return null;
        }

        Debug.Log($"CardViewCreator: Creating card view for {cardModel.Title} at {position}");
        CardView cardView = Instantiate(cardViewPrefab, position, rotation);
        cardView.transform.localScale = Vector3.zero;
        cardView.transform.DOScale(Vector3.one, duration);
        cardView.Setup(cardModel);
        return cardView;
    }
}
