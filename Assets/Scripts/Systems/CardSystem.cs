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

    public override void Init()
    {
        Debug.Log("CardSystem Init");
        ActionSystem.AttachPerformer<DrawCardsGA>(DrawCardsPerformer);
        ActionSystem.AttachPerformer<DiscardAllCardsGA>(DiscardAllCardsPerformer);
        ActionSystem.AttachPerformer<PlayCardGA>(PlayCardPerformer);
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.PRE);
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
        GameSystem.Instance.AddListener<PlayerController>(GameSystem.GameEvent.PlayerSelectedChanged, UpdateSelectedPlayer);
        base.Init();
    }

    public override void Stop()
    {
        if (Initialized)
        {
            ActionSystem.DetachPerformer<DrawCardsGA>();
            ActionSystem.DetachPerformer<DiscardAllCardsGA>();
            ActionSystem.DetachPerformer<PlayCardGA>();
            ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.PRE);
            ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
            GameSystem.Instance.RemoveListener<PlayerController>(GameSystem.GameEvent.PlayerSelectedChanged, UpdateSelectedPlayer);
        }

        base.Stop();
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
    }

    private void UpdateSelectedPlayer(PlayerController selectedPlayer)
    {
        this.selectedPlayer = selectedPlayer;
    }

    private bool Validate(string debugString)
    {
        if (selectedPlayer == null)
        {
            Debug.Log($"CardSystem Validate ({debugString}): selectedPlayer is null");
            return false;
        }

        if (!discardPiles.ContainsKey(selectedPlayer.name))
        {
            Debug.Log($"CardSystem Validate ({debugString}): {selectedPlayer.name} not found in discard piles");
            return false;
        }

        if (!drawPiles.ContainsKey(selectedPlayer.name))
        {
            Debug.Log($"CardSystem Validate ({debugString}): {selectedPlayer.name} not found in draw piles");
            return false;
        }

        if (!hands.ContainsKey(selectedPlayer.name))
        {
            Debug.Log($"CardSystem Validate ({debugString}): {selectedPlayer.name} not found in hands");
            return false;
        }

        return true;
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

    private IEnumerator PlayCardPerformer(PlayCardGA playCardGA)
    {
        if (!Validate("PlayCardPerformer")) yield break;
        hands[selectedPlayer.name].Remove(playCardGA.Card);
        CardView cardView = handView.RemoveCard(playCardGA.Card);
        yield return DiscardCard(cardView);

        // Perform effects
    }

    private IEnumerator DrawCardsPerformer(DrawCardsGA drawCardsGA)
    {
        if (!Validate("DrawCardsPerformer")) yield break;
        int actualAmount = Mathf.Min(drawCardsGA.Amount, drawPiles[selectedPlayer.name].Count);
        int notDrawnAmount = drawCardsGA.Amount - actualAmount;
        Debug.Log($"DrawCardsPerformer: {drawCardsGA.Amount} cards");
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
        if (!Validate("RefillDeck")) return;
        drawPiles[selectedPlayer.name].AddRange(discardPiles[selectedPlayer.name]);
        discardPiles[selectedPlayer.name].Clear();
    }

    private IEnumerator DrawCard()
    {
        if (!Validate("DrawCard")) yield break;
        CardModel card = drawPiles[selectedPlayer.name].Draw();
        Debug.Log($"CardSystem: Drawing card {card.Title}");
        hands[selectedPlayer.name].Add(card);
        CardView cardView = CardViewCreator.Instance.CreateCardView(card, drawPilePoint.position, drawPilePoint.rotation, duration);
        if (cardView == null)
        {
            Debug.LogError("CardSystem: Failed to create card view!");
            yield break;
        }
        Debug.Log($"CardSystem: Adding card {card.Title} to hand");
        yield return handView.AddCard(cardView);
    }

    private IEnumerator DiscardAllCardsPerformer(DiscardAllCardsGA discardAllCardsGA)
    {
        if (!Validate("DiscardAllCardsPerformer")) yield break;
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
