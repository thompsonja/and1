using UnityEngine;

public class PlayCardGA : GameAction
{
    public CardModel Card { get; set; }

    public PlayCardGA(CardModel card)
    {
        Card = card;
    }
}
