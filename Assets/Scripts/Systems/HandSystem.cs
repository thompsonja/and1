using System;
using System.Collections.Generic;
using UnityEngine;

public class HandSystem : Singleton<HandSystem>
{
    [SerializeField] private HandView handView;
    private PlayerController selectedPlayer = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameSystem.Instance.AddListener<PlayerController>(GameSystem.GameEvent.PlayerSelectedChanged, UpdateSelectedPlayer);
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