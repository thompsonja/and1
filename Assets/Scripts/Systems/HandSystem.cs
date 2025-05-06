using UnityEngine;

public class HandSystem : Singleton<HandSystem>
{
    [SerializeField] private HandView handView;
    private PlayerController selectedPlayer = null;

    public override void Init()
    {
        Debug.Log("HandSystem Init");
        GameSystem.Instance.AddListener<PlayerController>(GameSystem.GameEvent.PlayerSelectedChanged, UpdateSelectedPlayer);
        base.Init();
    }

    public override void Stop()
    {
        if (Initialized)
        {
            GameSystem.Instance.RemoveListener<PlayerController>(GameSystem.GameEvent.PlayerSelectedChanged, UpdateSelectedPlayer);
        }
        base.Stop();
    }

    private void UpdateSelectedPlayer(PlayerController selectedPlayer)
    {
        this.selectedPlayer = selectedPlayer;
    }

    public void DrawCard(CardData cardData)
    {
        if (selectedPlayer == null) return;
        CardModel cardModel = new(cardData);
        CardView cardView = CardViewCreator.Instance.CreateCardView(cardModel, handView.transform.position, Quaternion.identity, 0.15f);
        StartCoroutine(handView.AddCard(cardView));
    }
}