using UnityEngine;

public class Interactions : Singleton<Interactions>
{
    public bool PlayerIsDragging { get; set; } = false;

    public override void Init()
    {
        Debug.Log("Interactions Init");
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
