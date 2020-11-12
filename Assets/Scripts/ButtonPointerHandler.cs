using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.Collections;

/// <summary>
/// Custom-klass för att hantera knappar och interaktioner kring detta.
/// Axel Gustafsson, axgu8924
/// </summary>
public class ButtonPointerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    //färg som används för att skapa en "highlight"-effekt på hover
    [SerializeField]Color highlightColor;
    [SerializeField]Color original;

    Text[] texts;
    Image[] images;

    GameObject gradient;
    Image gradientImage;

    bool isAnimatingFeedback = false;

    void Start()
    {
        //cachea objekt som kan cacheas
        texts = this.GetComponentsInChildren<Text>();
        images = this.GetComponentsInChildren<Image>();

        //cachea och göm highlightgradient
        gradient = this.transform.Find("Gradient").gameObject;
        gradientImage = gradient.GetComponent<Image>();
        gradient.SetActive(false);
    }

    //implementera Enter, Click och Exit ifrån IPointerHandlers för att kunna lyssna på dessa events
    //dessa metoder fungerar som wrappers för den här klassens events, och invokerar dem ifall det finns lyssnare registrerade
    //sköter också lite logik som att byta mellan original/highlightfärger på enter/exit
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

    //kan användas för att skapa en animerad feedback om så önskas
    public void OnAnswerFeedbackStart(bool isCorrect)
    {
        gradient.SetActive(true);
        gradientImage.color = isCorrect ? Color.green : Color.red;

        isAnimatingFeedback = true;

        this.StartCoroutine(AnimateFeedback());
    }

    //animerar feedbacken för mer behaglig UX
    IEnumerator AnimateFeedback()
    {
        Color targetColor = gradientImage.color;
        Color transparent = new Color(targetColor.r, targetColor.g, targetColor.b, 0f);

        float t = 0f;

        while (t <= 2f)
        {
            t += Time.deltaTime;

            //linjär interpolering, höj upp t med 3 för att få en icke-linjär kurva (sk "smootherstep"/"fadein")
            gradientImage.color = Color.Lerp(targetColor, transparent, t * t * t);

            yield return null;
        }

        gradient.SetActive(false);
        isAnimatingFeedback = false;
    }

    //deklarera events
    public delegate void PointerEvent(ButtonPointerHandler handler);
    public event PointerEvent OnEnter;
    public event PointerEvent OnClick;
    public event PointerEvent OnExit;
}
