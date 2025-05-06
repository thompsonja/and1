using System.Collections;
using UnityEngine;

public class EnemySystem : Singleton<EnemySystem>
{
    public override void Init()
    {
        Debug.Log("EnemySystem Init");
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
        Debug.Log("Enemy Turn");
        yield return new WaitForSeconds(2f);
        Debug.Log("Enemy Turn Ended");
    }
}
