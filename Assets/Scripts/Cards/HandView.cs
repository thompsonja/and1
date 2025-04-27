using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;

public class HandView : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;
    private readonly Dictionary<PlayerController, PlayerHand> playerHands = new();
    public int maxCards = 10;
    public float duration = 0.15f;
    public GameObject handPrefab;

    private void Start()
    {
        GameSystem.Instance.AddListener<PlayerController>(GameSystem.GameEvent.PlayerSelectedChanged, OnPlayerSelectedChanged);
    }

    private void OnDestroy()
    {
        if (GameSystem.Instance)
        {
            GameSystem.Instance.RemoveListener<PlayerController>(GameSystem.GameEvent.PlayerSelectedChanged, OnPlayerSelectedChanged);
        }
    }

    private void OnPlayerSelectedChanged(PlayerController player)
    {
        // Hide all hands
        foreach (var hand in playerHands.Values)
        {
            hand.Hide();
        }

        // Show selected player's hand
        if (player != null && playerHands.TryGetValue(player, out var selectedHand))
        {
            selectedHand.Show();
        }
    }

    public void RegisterPlayer(PlayerController player)
    {
        if (!playerHands.ContainsKey(player))
        {
            GameObject handObject = Instantiate(handPrefab, transform);
            PlayerHand hand = handObject.GetComponent<PlayerHand>();
            hand.maxCards = maxCards;
            hand.parent = handObject;
            playerHands[player] = hand;
            hand.Hide(); // Start hidden
        }
    }

    public IEnumerator AddCard(PlayerController player, CardView cardView)
    {
        if (playerHands.TryGetValue(player, out var hand))
        {
            yield return hand.AddCard(cardView);
        }
    }
}
