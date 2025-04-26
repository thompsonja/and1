using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter");
        Debug.Log(transform.position);
        Vector3 pos = transform.position + Vector3.up * 50;

        CardViewHoverSystem.Instance.Show(CardModel, pos);
        // wrapper.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CardViewHoverSystem.Instance.Hide(transform.position);
        // wrapper.SetActive(true);
    }
}
