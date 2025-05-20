using System.Collections;
using UnityEngine;

public class EnemySystem : Singleton<EnemySystem>
{
    public override void Init(string instanceName, LogLevel level)
    {
        base.Init(instanceName, level);
        ActionSystem.AttachPerformer<EnemyTurnGA>(EnemyTurnPerformer);
        InitComplete();
    }

    public override void Stop()
    {
        if (Initialized)
        {
            ActionSystem.DetachPerformer<EnemyTurnGA>();
        }
        base.Stop();
    }

    // Performers

    private IEnumerator EnemyTurnPerformer(EnemyTurnGA enemyTurnGA)
    {
        LogInfo("Enemy Turn");
        yield return new WaitForSeconds(2f);
        LogInfo("Enemy Turn Ended");
    }
}
