using UnityEngine;
using UnityEngine.UI;

public class CardModel
{
    public string Title => data.Title;
    public string Description => data.Description;
    public Sprite Image => data.Image;
    public int Energy { get; private set; }

    private readonly CardData data;
    public CardModel(CardData cardData)
    {
        data = cardData;
        Energy = cardData.Energy;
    }
}