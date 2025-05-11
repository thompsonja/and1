using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class MatchSetupSystem : MonoBehaviour
{
    [Header("Match Settings")]
    [SerializeField] private List<CardData> deckData;
    [SerializeField] private int startingHandSize;

    private bool initialized = false;

    private void initializeSubsystems()
    {
        if (initialized) return;

        GameSystem.Instance.Init();
        ActionSystem.Instance.Init();
        CardSystem.Instance.Init();
        CardViewHoverSystem.Instance.Init();
        CardViewCreator.Instance.Init();
        EnemySystem.Instance.Init();
        HandSystem.Instance.Init();
        UISystem.Instance.Init();
        Interactions.Instance.Init();
        initialized = true;
    }

    void Start()
    {
        Debug.Log("Match begun");
        initializeSubsystems();

        PlayerController[] controllers = FindObjectsByType<PlayerController>(FindObjectsSortMode.InstanceID);
        foreach (var c in controllers)
        {
            CardSystem.Instance.Setup(c, deckData);
        }

        var player1 = controllers.Where(c => c.name == "Player 1").First();

        Debug.Log($"Match system setting player to {player1.name}");
        GameSystem.Instance.SetSelectedPlayer(player1);

        // Wait for next frame to ensure all systems are properly initialized
        StartCoroutine(DrawStartingHand());
    }

    private IEnumerator DrawStartingHand()
    {
        yield return null; // Wait for next frame
        ActionSystem.Instance.Perform(new DrawCardsGA(startingHandSize));
    }

    void OnApplicationQuit()
    {
        if (!initialized) return;
        Interactions.Instance?.Stop();
        UISystem.Instance?.Stop();
        HandSystem.Instance?.Stop();
        EnemySystem.Instance?.Stop();
        CardSystem.Instance?.Stop();
        CardViewHoverSystem.Instance?.Stop();
        CardViewCreator.Instance?.Stop();
        ActionSystem.Instance?.Stop();
        GameSystem.Instance?.Stop();
    }
}
