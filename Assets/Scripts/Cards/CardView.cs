using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text energy;
    [SerializeField] private Image image;
    [SerializeField] private GameObject wrapper;

    public CardModel CardModel { get; private set; }
    public void Setup(CardModel cardModel)
    {
        CardModel = cardModel;
        title.text = cardModel.Title;
        description.text = cardModel.Description;
        energy.text = cardModel.Energy.ToString();
        image.sprite = cardModel.Image;
    }
}
