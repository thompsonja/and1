using UnityEngine;

public class DrawCardsGA : GameAction
{
    public int Amount { get; set; }
    public string PlayerName { get; set; }
    public DrawCardsGA(int amount, string playerName)
    {
        Amount = amount;
        PlayerName = playerName;
    }
}
