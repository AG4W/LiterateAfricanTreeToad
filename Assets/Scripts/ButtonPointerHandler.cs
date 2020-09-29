using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.Collections;

public class ButtonPointerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]Color highlightColor;
    [SerializeField]Color original;

    Text[] texts;
    Image[] images;

    GameObject gradient;
    Image gradientImage;

    bool isAnimatingFeedback = false;

    void Start()
    {
        texts = this.GetComponentsInChildren<Text>();
        images = this.GetComponentsInChildren<Image>();

        gradient = this.transform.Find("Gradient").gameObject;
        gradientImage = gradient.GetComponent<Image>();
        gradient.SetActive(false);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        OnEnter?.Invoke(this);

        if (!isAnimatingFeedback)
        {
            for (int i = 0; i < texts.Length; i++)
                texts[i].color = highlightColor;
            for (int i = 0; i < images.Length; i++)
                images[i].color = highlightColor;

            gradient.SetActive(true);
        }
    }
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(this);
    }
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        OnExit?.Invoke(this);

        if (!isAnimatingFeedback)
        {
            for (int i = 0; i < texts.Length; i++)
                texts[i].color = original;
            for (int i = 0; i < images.Length; i++)
                images[i].color = original;

            gradient.SetActive(false);
        }
    }

    public void ClearClickListeners() => OnClick = null;

    public void OnAnswerFeedbackStart(bool isCorrect)
    {
        gradient.SetActive(true);
        gradientImage.color = isCorrect ? Color.green : Color.red;

        isAnimatingFeedback = true;

        this.StartCoroutine(AnimateFeedback());
    }

    IEnumerator AnimateFeedback()
    {
        Color targetColor = gradientImage.color;
        Color transparent = new Color(targetColor.r, targetColor.g, targetColor.b, 0f);

        float t = 0f;

        while (t <= 2f)
        {
            t += Time.deltaTime;
            gradientImage.color = Color.Lerp(targetColor, transparent, t * t * t);

            yield return null;
        }

        gradient.SetActive(false);
        isAnimatingFeedback = false;
    }

    public delegate void PointerEvent(ButtonPointerHandler handler);
    public event PointerEvent OnEnter;
    public event PointerEvent OnClick;
    public event PointerEvent OnExit;
}
