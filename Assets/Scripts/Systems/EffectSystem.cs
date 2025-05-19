using System.Collections;
using UnityEngine;

public class EffectSystem : Singleton<EffectSystem>
{
    public override void Init()
    {
        Debug.Log("EffectSystem Init");
        base.Init();
        ActionSystem.AttachPerformer<PerformEffectGA>(PerformEffectPerformer);
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
