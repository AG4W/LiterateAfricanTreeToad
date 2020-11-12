using UnityEngine;
using UnityEngine.UI;

using System;

/// <summary>
/// Klass som styr popupfönstrets funktionalitet
/// </summary>
public class PopupController : MonoBehaviour
{
    //singleton för att slippa referenshantering överallt
    public static PopupController GetInstance { get; private set; }

    [SerializeField]GameObject window;
    [SerializeField]Text question;

    //första knappen
    [Header("Accept")]
    [SerializeField]ButtonPointerHandler acceptHandler;
    [SerializeField]Text accept;

    //andra knappen
    [Header("Decline")]
    [SerializeField]ButtonPointerHandler declineHandler;
    [SerializeField]Text decline;

    void Awake()
    {
        //cachea singelton
        GetInstance = this;

        window.SetActive(false);
    }

    //visa popupfönstret
    public void Open(string question, string accept, Action onAccept, string decline, Action onDecline)
    {
        //dåligt variabelnamn, men det är själva innehållet i fönstret
        this.question.text = question;

        //registrera vad som ska hända när man klickar på knapp 1, och sätt upp text
        //gömmer också knappen ifall ingen aktion är satt.
        this.acceptHandler.OnClick += (ButtonPointerHandler h) => onAccept();
        this.acceptHandler.OnClick += (ButtonPointerHandler h) => Close();
        this.accept.text = accept;
        this.acceptHandler.gameObject.SetActive(onAccept != null);

        //registrera vad som ska hända när man klickar på knapp 2, och sätt upp text
        //gömmer också knappen ifall ingen aktion är satt.
        this.declineHandler.OnClick += (ButtonPointerHandler h) => onDecline();
        this.declineHandler.OnClick += (ButtonPointerHandler h) => Close();
        this.decline.text = decline;
        this.declineHandler.gameObject.SetActive(onDecline != null);

        //visa fönstret
        this.window.SetActive(true);
    }
    public void Close()
    {
        //rensa lyssnare
        this.acceptHandler.ClearClickListeners();
        this.declineHandler.ClearClickListeners();

        //göm fönstret
        this.window.SetActive(false);
    }
}
