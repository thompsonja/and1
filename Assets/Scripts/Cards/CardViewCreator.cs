using DG.Tweening;
using UnityEngine;

public class CardViewCreator : Singleton<CardViewCreator>
{
    [SerializeField] private CardView cardViewPrefab;

    public override void Init(string instanceName, LogLevel level)
    {
        base.Init(instanceName, level);
        if (cardViewPrefab == null)
        {
            LogError("cardViewPrefab is not assigned!");
        }
        InitComplete();
    }

    public CardView CreateCardView(CardModel cardModel, Vector3 position, Quaternion rotation, float duration)
    {
        if (cardViewPrefab == null)
        {
            LogError("Cannot create card view - prefab is null!");
            return null;
        }

        LogInfo($"Creating card view for {cardModel.Title} at {position}");
        CardView cardView = Instantiate(cardViewPrefab, position, rotation);
        cardView.transform.localScale = Vector3.zero;
        cardView.transform.DOScale(Vector3.one, duration);
        cardView.Setup(cardModel);
        return cardView;
    }
}
