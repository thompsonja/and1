using UnityEngine;

public class PlayCardGA : GameAction
{
    public CardModel Card { get; set; }

    // If null, PlayerName refers to the selected player
    public string PlayerName { get; set; }

    public PlayCardGA(CardModel card, string playerName)
    {
        Card = card;
        PlayerName = playerName;
    }
}
