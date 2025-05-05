using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestSystem : MonoBehaviour
{
    [SerializeField] private List<CardData> deckData;
    [SerializeField] private int startingHandSize;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerController[] controllers = FindObjectsByType<PlayerController>(FindObjectsSortMode.InstanceID);
        foreach (var c in controllers)
        {
            CardSystem.Instance.Setup(c, deckData);
        }

        GameSystem.Instance.SetSelectedPlayer(controllers.Where(c => c.name == "Player 1").First());

        ActionSystem.Instance.Perform(new DrawCardsGA(startingHandSize));
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     HandSystem.Instance.DrawCard(cardData);
        // }
    }
}
