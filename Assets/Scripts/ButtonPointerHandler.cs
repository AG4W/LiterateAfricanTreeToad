using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonPointerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]Color highlightColor;
    [SerializeField]Color original;

    Text[] texts;
    Image[] images;

    void Start()
    {
        texts = this.GetComponentsInChildren<Text>();
        images = this.GetComponentsInChildren<Image>();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        OnEnter?.Invoke(this);

        for (int i = 0; i < texts.Length; i++)
            texts[i].color = highlightColor;
        for (int i = 0; i < images.Length; i++)
            images[i].color = highlightColor;
    }
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(this);
    }
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        OnExit?.Invoke(this);

        for (int i = 0; i < texts.Length; i++)
            texts[i].color = original;
        for (int i = 0; i < images.Length; i++)
            images[i].color = original;
    }


    public delegate void PointerEvent(ButtonPointerHandler handler);
    public event PointerEvent OnEnter;
    public event PointerEvent OnClick;
    public event PointerEvent OnExit;
}
