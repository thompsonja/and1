using System.Collections;
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
        Vector3 pos = transform.position + Vector3.up * 50;

        CardViewHoverSystem.Instance.Show(CardModel, pos);

        var cg = wrapper.GetComponent<CanvasGroup>();
        cg.alpha = 0;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CardViewHoverSystem.Instance.Hide(transform.position);
        var cg = wrapper.GetComponent<CanvasGroup>();
        cg.alpha = 1;
    }
}
