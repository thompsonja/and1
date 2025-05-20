using System;
using System.Collections;
using System.Collections.Generic;

public class ActionSystem : Singleton<ActionSystem>
{
    private List<GameAction> reactions = null;

    public bool IsPerforming { get; private set; } = false;

    // Reaction subscribers subscribed to before game actions
    private static Dictionary<Type, List<Action<GameAction>>> preSubs = new();

    // Reaction subscribers subscribed to after game actions
    private static Dictionary<Type, List<Action<GameAction>>> postSubs = new();

    // Hold logic for game actions
    private static Dictionary<Type, Func<GameAction, IEnumerator>> performers = new();

    public override void Init(string instanceName, LogLevel level)
    {
        base.Init(instanceName, level);
        InitComplete();
    }

    public void Perform(GameAction action, Action OnPerformFinished = null)
    {
        if (IsPerforming) return;
        IsPerforming = true;
        StartCoroutine(Flow(action, () =>
        {
            IsPerforming = false;
            OnPerformFinished?.Invoke();
        }));
    }

    private IEnumerator Flow(GameAction action, Action OnFlowFinished = null)
    {
        // Set reactions to pre reactions for current action, so that
        // all objects in game can react to current reaction before it is performed.
        reactions = action.PreReactions;
        PerformSubscribers(action, preSubs);
        yield return PerformReactions();

        reactions = action.PerformReactions;
        yield return PerformPerformer(action);
        yield return PerformReactions();

        reactions = action.PostReactions;
        PerformSubscribers(action, postSubs);
        yield return PerformReactions();

        OnFlowFinished?.Invoke();
    }

    public void AddReaction(GameAction gameAction)
    {
        reactions?.Add(gameAction);
    }

    private IEnumerator PerformReactions()
    {
        foreach (var reaction in reactions)
        {
            yield return Flow(reaction);
        }
    }

    private void PerformSubscribers(GameAction action, Dictionary<Type, List<Action<GameAction>>> subs)
    {
        Type type = action.GetType();
        if (subs.ContainsKey(type))
        {
            foreach (var sub in subs[type])
            {
                sub(action);
            }
        }
    }

    private IEnumerator PerformPerformer(GameAction action)
    {
        Type type = action.GetType();
        if (performers.ContainsKey(type))
        {
            yield return performers[type](action);
        }
    }

    public static void AttachPerformer<T>(Func<T, IEnumerator> performer) where T : GameAction
    {
        Type type = typeof(T);
        IEnumerator wrappedPerformer(GameAction action) => performer((T)action);
        if (!performers.ContainsKey(type))
        {
            performers[type] = wrappedPerformer;
        }
        else
        {
            performers.Add(type, wrappedPerformer);
        }
    }

    public static void DetachPerformer<T>() where T : GameAction
    {
        Type type = typeof(T);
        if (performers.ContainsKey(type))
        {
            performers.Remove(type);
        }
    }

    public static void SubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        Type type = typeof(T);
        void wrappedReaction(GameAction action) => reaction((T)action);

        var subs = timing == ReactionTiming.PRE ? preSubs : postSubs;
        if (!subs.ContainsKey(type))
        {
            subs.Add(type, new());
        }
        subs[type].Add(wrappedReaction);
    }

    public static void UnsubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        Type type = typeof(T);
        var subs = timing == ReactionTiming.PRE ? preSubs : postSubs;
        if (subs.ContainsKey(type))
        {
            void wrappedReaction(GameAction action) => reaction((T)action);
            subs[type].Remove(wrappedReaction);
        }
    }
}