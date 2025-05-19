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
        EffectSystem.Instance.Init();
        EnemySystem.Instance.Init();
        UISystem.Instance.Init();
        Interactions.Instance.Init();
        initialized = true;

        GameSystem.Instance.MinimumLogLevel = LogLevel.WARN;
        ActionSystem.Instance.MinimumLogLevel = LogLevel.WARN;
        CardSystem.Instance.MinimumLogLevel = LogLevel.WARN;
        CardViewHoverSystem.Instance.MinimumLogLevel = LogLevel.WARN;
        EffectSystem.Instance.MinimumLogLevel = LogLevel.WARN;
        EnemySystem.Instance.MinimumLogLevel = LogLevel.WARN;
        UISystem.Instance.MinimumLogLevel = LogLevel.WARN;
        Interactions.Instance.MinimumLogLevel = LogLevel.WARN;
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
        StartCoroutine(DrawStartingHand(controllers));
    }

    private IEnumerator DrawStartingHand(PlayerController[] controllers)
    {
        yield return null; // Wait for next frame
        foreach (var p in controllers)
        {
            bool actionComplete = false;
            ActionSystem.Instance.Perform(new DrawCardsGA(startingHandSize, p.name), () => actionComplete = true);
            yield return new WaitUntil(() => actionComplete);
        }
    }

    void OnApplicationQuit()
    {
        if (!initialized) return;
        Interactions.Instance?.Stop();
        UISystem.Instance?.Stop();
        EnemySystem.Instance?.Stop();
        EffectSystem.Instance?.Stop();
        CardSystem.Instance?.Stop();
        CardViewHoverSystem.Instance?.Stop();
        CardViewCreator.Instance?.Stop();
        ActionSystem.Instance?.Stop();
        GameSystem.Instance?.Stop();
    }
}
