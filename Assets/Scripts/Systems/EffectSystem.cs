using System.Collections;
using UnityEngine;

public class EffectSystem : Singleton<EffectSystem>
{
    public override void Init(string instanceName, LogLevel level)
    {
        base.Init(instanceName, level);
        ActionSystem.AttachPerformer<PerformEffectGA>(PerformEffectPerformer);
        InitComplete();
    }

    public override void Stop()
    {
        if (Initialized)
        {
            ActionSystem.DetachPerformer<PerformEffectGA>();
        }
        base.Stop();
    }

    // Performers

    private IEnumerator PerformEffectPerformer(PerformEffectGA performEffectGA)
    {
        GameAction effectAction = performEffectGA.Effect.GetGameAction();
        ActionSystem.Instance.AddReaction(effectAction);
        yield return null;
    }
}
