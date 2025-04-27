using System;
using System.Collections.Generic;

public class GameSystem : Singleton<GameSystem>
{
    public enum GameEvent
    {
        PlayerSelectedChanged,
    }

    private PlayerController selectedPlayer;

    public void SetSelectedPlayer(PlayerController player)
    {
        selectedPlayer = player;
        TriggerEvent(GameEvent.PlayerSelectedChanged, player);
    }

    private Dictionary<GameEvent, List<Delegate>> events = new();

    public void AddListener<T>(GameEvent gameEvent, Action<T> listener)
    {
        if (!events.ContainsKey(gameEvent))
        {
            events[gameEvent] = new();
        }
        events[gameEvent].Add(listener);
    }

    public void RemoveListener<T>(GameEvent gameEvent, Action<T> listener)
    {
        if (events.ContainsKey(gameEvent))
        {
            events[gameEvent].Remove(listener);
        }
    }

    public void TriggerEvent<T>(GameEvent gameEvent, T eventData)
    {
        if (!events.ContainsKey(gameEvent)) return;
        foreach (var d in events[gameEvent])
        {
            if (d is Action<T> action)
            {
                action?.Invoke(eventData);
            }
        }
    }
}