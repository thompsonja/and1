using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text energy;
    [SerializeField] private Image image;
    [SerializeField] private GameObject wrapper;

    public CardModel CardModel { get; private set; }

    private Vector2 dragOffset;
    private Vector3 dragStartPosition;
    private Quaternion dragStartRotation;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    public void Setup(CardModel cardModel)
    {
        CardModel = cardModel;
        title.text = cardModel.Title;
        description.text = cardModel.Description;
        energy.text = cardModel.Energy.ToString();
        image.sprite = cardModel.Image;

        canvasGroup = wrapper.GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!Interactions.Instance.PlayerCanHover()) return;

        Vector3 pos = transform.position + Vector3.up * 50;

        CardViewHoverSystem.Instance.Show(CardModel, pos);
        canvasGroup.alpha = 0;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!Interactions.Instance.PlayerCanHover()) return;

        CardViewHoverSystem.Instance.Hide(transform.position);
        canvasGroup.alpha = 1;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;
        Interactions.Instance.PlayerIsDragging = true;

        CardViewHoverSystem.Instance.Hide(transform.position);
        canvasGroup.alpha = 1;

        var parentRectTransform = rectTransform.parent as RectTransform;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, eventData.position, null, out dragOffset);

        dragStartPosition = rectTransform.anchoredPosition3D;
        dragStartRotation = rectTransform.localRotation;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;
        var parentRectTransform = rectTransform.parent as RectTransform;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, eventData.position, null, out Vector2 localMousePosition))
        {
            rectTransform.anchoredPosition = localMousePosition - dragOffset;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;
        if (Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, 10f))
        {
            PlayCardGA playCardGA = new(CardModel);
            ActionSystem.Instance.Perform(playCardGA);
        }
        else
        {
            rectTransform.anchoredPosition3D = dragStartPosition;
            rectTransform.localRotation = dragStartRotation;
        }
        Interactions.Instance.PlayerIsDragging = false;
    }
}
