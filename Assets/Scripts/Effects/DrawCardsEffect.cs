using System;
using UnityEngine;

[Serializable]
public class DrawCardsEffect : Effect
{
    [SerializeField] public int drawAmount;
    [SerializeField] public string playerName;

    public override GameAction GetGameAction()
    {
        return new DrawCardsGA(drawAmount, playerName);
    }
}
