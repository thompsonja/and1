using System.Collections;
using UnityEngine;

public class EnemySystem : BaseSystem<EnemySystem>
{
    public override void Init()
    {
        LogInfo("EnemySystem Init");
        base.Init();
        ActionSystem.AttachPerformer<EnemyTurnGA>(EnemyTurnPerformer);
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
