using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System;

public class PopupController : MonoBehaviour
{
    public static PopupController GetInstance { get; private set; }

    [SerializeField]GameObject window;
    [SerializeField]Text question;

    [Header("Accept")]
    [SerializeField]ButtonPointerHandler acceptHandler;
    [SerializeField]Text accept;

    [Header("Decline")]
    [SerializeField]ButtonPointerHandler declineHandler;
    [SerializeField]Text decline;

    void Awake()
    {
        GetInstance = this;

        window.SetActive(false);
    }

    public void Open(string question, string accept, Action onAccept, string decline, Action onDecline)
    {
        this.question.text = question;

        this.acceptHandler.OnClick += (ButtonPointerHandler h) => onAccept();
        this.acceptHandler.OnClick += (ButtonPointerHandler h) => Close();
        this.accept.text = accept;
        this.acceptHandler.gameObject.SetActive(onAccept != null);

        this.declineHandler.OnClick += (ButtonPointerHandler h) => onDecline();
        this.declineHandler.OnClick += (ButtonPointerHandler h) => Close();
        this.decline.text = decline;
        this.declineHandler.gameObject.SetActive(onDecline != null);

        this.window.SetActive(true);
    }
    public void Close()
    {
        this.acceptHandler.ClearClickListeners();
        this.declineHandler.ClearClickListeners();

        this.window.SetActive(false);
    }
}
