using UnityEngine;

public class Interactions : BaseSystem<Interactions>
{
    public bool PlayerIsDragging { get; set; } = false;

    public override void Init()
    {
        LogInfo("Interactions Init");
        base.Init();
    }

    public override void Stop()
    {
        base.Stop();
    }

    public bool PlayerCanInteract()
    {
        return !ActionSystem.Instance.IsPerforming;
    }

    public bool PlayerCanHover()
    {
        return !PlayerIsDragging;
    }

}
