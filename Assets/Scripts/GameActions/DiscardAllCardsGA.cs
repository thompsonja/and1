public class DiscardAllCardsGA : GameAction
{
    public string PlayerName { get; set; }
    public DiscardAllCardsGA(string playerName)
    {
        PlayerName = playerName;
    }
}
