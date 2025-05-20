using TMPro;
using UnityEngine;

public class UISystem : Singleton<UISystem>
{
    public TMP_Text selectedPlayerName;

    public override void Init(string instanceName, LogLevel level)
    {
        base.Init(instanceName, level);
        GameSystem.Instance.AddListener<PlayerController>(GameSystem.GameEvent.PlayerSelectedChanged, UpdateSelectedPlayer);
        InitComplete();
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