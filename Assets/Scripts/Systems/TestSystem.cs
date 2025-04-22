using UnityEngine;

public class TestSystem : MonoBehaviour
{
    public GameObject parent;

    [SerializeField] private HandView handView;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CardView cardView = CardViewCreator.Instance.CreateCardView(transform.position, Quaternion.identity, 0.15f);
            StartCoroutine(handView.AddCard(cardView));
        }
    }
}
