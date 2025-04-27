using TMPro;

public class UISystem : Singleton<UISystem>
{
    public TMP_Text selectedPlayerName;

    private void Start()
    {
        GameSystem.Instance.AddListener<PlayerController>(GameSystem.GameEvent.PlayerSelectedChanged, UpdateSelectedPlayer);
    }

    private void OnDisable()
    {
        if (!GameSystem.Instance) return;
        GameSystem.Instance.RemoveListener<PlayerController>(GameSystem.GameEvent.PlayerSelectedChanged, UpdateSelectedPlayer);
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