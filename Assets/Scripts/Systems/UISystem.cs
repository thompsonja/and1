using TMPro;
using UnityEngine;

public class UISystem : BaseSystem<UISystem>
{
    public TMP_Text selectedPlayerName;

    public override void Init()
    {
        LogInfo("UISystem Init");
        GameSystem.Instance.AddListener<PlayerController>(GameSystem.GameEvent.PlayerSelectedChanged, UpdateSelectedPlayer);
        base.Init();
    }

    public override void Stop()
    {
        if (Initialized)
        {
            GameSystem.Instance.RemoveListener<PlayerController>(GameSystem.GameEvent.PlayerSelectedChanged, UpdateSelectedPlayer);
        }
        base.Stop();
    }

    private void UpdateSelectedPlayer(PlayerController selectedPlayer)
    {
        if (selectedPlayer == null)
        {
            selectedPlayerName.text = "No player selected";
        }
        else
        {
            selectedPlayerName.text = selectedPlayer.name;
        }
    }
}