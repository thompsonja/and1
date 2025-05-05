using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class CardSystem : Singleton<CardSystem>
{
    [SerializeField] private HandView handView;
    [SerializeField] private Transform drawPilePoint;
    [SerializeField] private Transform discardPilePoint;
    [SerializeField] private float duration;

    private readonly Dictionary<string, List<CardModel>> drawPiles = new();
    private readonly Dictionary<string, List<CardModel>> discardPiles = new();
    private readonly Dictionary<string, List<CardModel>> hands = new();
    private PlayerController selectedPlayer = null;

    void OnEnable()
    {
        ActionSystem.AttachPerformer<DrawCardsGA>(DrawCardsPerformer);
        ActionSystem.AttachPerformer<DiscardAllCardsGA>(DiscardAllCardsPerformer);
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.PRE);
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<DrawCardsGA>();
        ActionSystem.DetachPerformer<DiscardAllCardsGA>();
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.PRE);
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }

    public void Setup(PlayerController controller, List<CardData> deckData)
    {
        drawPiles[controller.name] = new();
        discardPiles[controller.name] = new();
        hands[controller.name] = new();
        foreach (var card in deckData)
        {
            CardModel cardModel = new(card);
            drawPiles[controller.name].Add(cardModel);
        }
        Debug.Log($"Setup: {controller.name}, draw pile length {drawPiles[controller.name].Count}");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameSystem.Instance.AddListener<PlayerController>(GameSystem.GameEvent.PlayerSelectedChanged, UpdateSelectedPlayer);
    }

    private void UpdateSelectedPlayer(PlayerController selectedPlayer)
    {
        this.selectedPlayer = selectedPlayer;
    }

    // Reactions

    // Performed before enemy turn
    private void EnemyTurnPreReaction(EnemyTurnGA enemyTurnGA)
    {
        DiscardAllCardsGA discardAllCardsGA = new();
        ActionSystem.Instance.AddReaction(discardAllCardsGA);
    }

    // Performed after enemy turn
    private void EnemyTurnPostReaction(EnemyTurnGA enemyTurnGA)
    {
        DrawCardsGA drawCardsGA = new(5);
        ActionSystem.Instance.AddReaction(drawCardsGA);
    }

    // Performers

    private IEnumerator DrawCardsPerformer(DrawCardsGA drawCardsGA)
    {
        if (selectedPlayer == null || !drawPiles.ContainsKey(selectedPlayer.name))
        {
            Debug.Log($"DrawCardsPerformer: selectedPlayer is {selectedPlayer.name}");
        }
        int actualAmount = Mathf.Min(drawCardsGA.Amount, drawPiles[selectedPlayer.name].Count);
        int notDrawnAmount = drawCardsGA.Amount - actualAmount;
        for (int i = 0; i < actualAmount; i++)
        {
            yield return DrawCard();
        }
        if (notDrawnAmount > 0)
        {
            RefillDeck();
            for (int i = 0; i < notDrawnAmount; i++)
            {
                yield return DrawCard();
            }
        }
    }

    private void RefillDeck()
    {
        if (selectedPlayer == null || !drawPiles.ContainsKey(selectedPlayer.name))
        {
            Debug.Log($"RefillDeck: selectedPlayer is {selectedPlayer.name}");
        }
        drawPiles[selectedPlayer.name].AddRange(discardPiles[selectedPlayer.name]);
        discardPiles[selectedPlayer.name].Clear();
    }

    private IEnumerator DrawCard()
    {
        if (selectedPlayer == null || !drawPiles.ContainsKey(selectedPlayer.name) || !hands.ContainsKey(selectedPlayer.name))
        {
            Debug.Log($"DrawCard: selectedPlayer is {selectedPlayer.name}");
            yield break;
        }
        CardModel card = drawPiles[selectedPlayer.name].Draw();
        hands[selectedPlayer.name].Add(card);
        CardView cardView = CardViewCreator.Instance.CreateCardView(card, drawPilePoint.position, drawPilePoint.rotation, duration);
        Debug.Log($"Player {selectedPlayer.name} drew card {card}");
        yield return handView.AddCard(cardView);
    }

    private IEnumerator DiscardAllCardsPerformer(DiscardAllCardsGA discardAllCardsGA)
    {
        if (selectedPlayer == null || !hands.ContainsKey(selectedPlayer.name))
        {
            Debug.Log($"DiscardAllCardsPerformer: selectedPlayer is {selectedPlayer.name}");
            yield break;
        }
        foreach (var card in hands[selectedPlayer.name])
        {
            discardPiles[selectedPlayer.name].Add(card);
            CardView cardView = handView.RemoveCard(card);
            yield return DiscardCard(cardView);
        }
        hands[selectedPlayer.name].Clear();
    }

    private IEnumerator DiscardCard(CardView cardView)
    {
        cardView.transform.DOScale(Vector3.zero, duration);
        Tween tween = cardView.transform.DOMove(discardPilePoint.position, duration);
        yield return tween.WaitForCompletion();
        Destroy(cardView.gameObject);
    }
}
