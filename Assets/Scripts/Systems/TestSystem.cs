using UnityEngine;

public class TestSystem : MonoBehaviour
{
    [SerializeField] private HandView handView;
    [SerializeField] private CardData cardData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CardModel cardModel = new(cardData);
            CardView cardView = CardViewCreator.Instance.CreateCardView(cardModel, transform.position, Quaternion.identity, 0.15f);
            StartCoroutine(handView.AddCard(cardView));
        }
    }
}
