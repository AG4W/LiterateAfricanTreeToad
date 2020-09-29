using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

public class DialogueController : MonoBehaviour
{
    [SerializeField]AudioSource sfx;
    [SerializeField]AudioClip correct;
    [SerializeField]AudioClip incorrect;
    [SerializeField]AudioClip tick;

    [SerializeField]Transform brainRoot;

    [SerializeField]Transform answerList;
    [SerializeField]GameObject buttonPrefab;

    [SerializeField]Text blurb;

    [SerializeField]DialogueOption[] questions;

    [SerializeField]string[] factoids;

    DialogueOption current;
    List<DialogueOption> available;

    void Start()
    {
        available = new List<DialogueOption>(questions);

        SetupNewQuestion();
    }
    void SetupNewQuestion()
    {
        //pick new question
        current = available[UnityEngine.Random.Range(0, available.Count)];

        UpdateUI();
    }

    void UpdateUI()
    {
        for (int i = 0; i < answerList.childCount; i++)
            Destroy(answerList.GetChild(i).gameObject);
        for (int i = 0; i < brainRoot.childCount; i++)
            Destroy(brainRoot.GetChild(i).gameObject);

        if(SceneManager.GetActiveScene().name == "Spel1")
        {
            for (int i = 0; i < available.Count; i++)
            {
                DialogueOption option = available[i];

                GameObject g = Instantiate(buttonPrefab, answerList);
                GameObject p = Instantiate(option.CorrectBrainPart, brainRoot);
                MeshPointerHandler mph = p.GetComponentInChildren<MeshPointerHandler>();
                ButtonPointerHandler bph = g.transform.GetComponentInChildren<ButtonPointerHandler>();

                g.transform.Find("Text").GetComponentInChildren<Text>().text = available[i].name;
                bph.OnEnter += (ButtonPointerHandler h) => {
                    sfx.PlayOneShot(tick);
                    mph.OnEnter();
                };
                bph.OnClick += (ButtonPointerHandler h) => {
                    mph.OnExit();
                    OnClick(option, bph);
                };
                bph.OnExit += (ButtonPointerHandler h) => mph.OnExit();
            }
        }
        else
        {
            for (int i = 0; i < Enum.GetNames(typeof(BrainPart)).Length; i++)
            {
                BrainPart bp = (BrainPart)i;

                GameObject g = Instantiate(buttonPrefab, answerList);

                ButtonPointerHandler bph = g.transform.GetComponentInChildren<ButtonPointerHandler>();

                g.transform.Find("Text").GetComponentInChildren<Text>().text = Regex.Replace(bp.ToString(), "[A-Z]", " $0");
                bph.OnEnter += (ButtonPointerHandler h) => sfx.PlayOneShot(tick);
                bph.OnClick += (ButtonPointerHandler h) => OnClick(bp, bph);
            }
        }

        blurb.text = "<size=24><color=orange>Which part of the brain is this?</color></size>\n\n" + current.Description;

        if (SceneManager.GetActiveScene().name == "Spel2")
            brainRoot.gameObject.SetActive(false);
    }
    void OnClick(DialogueOption option, ButtonPointerHandler handler) => this.StartCoroutine(FeedbackWaitTimer(option, handler));
    void OnClick(BrainPart part, ButtonPointerHandler handler) => this.StartCoroutine(FeedbackWaitTimer(part, handler));

    IEnumerator FeedbackWaitTimer(DialogueOption option, ButtonPointerHandler handler)
    {
        handler.OnAnswerFeedbackStart(option.CorrectBrainPart == current.CorrectBrainPart);

        sfx.clip = option.CorrectBrainPart == current.CorrectBrainPart ? correct : incorrect;
        sfx.Play();

        yield return new WaitForSeconds(1f);

        if (option.CorrectBrainPart == current.CorrectBrainPart)
            available.Remove(current);

        if (available.Count > 0)
        {
            if (option.CorrectBrainPart == current.CorrectBrainPart)
                PopupController.GetInstance.Open(
                    "Du hade rätt!\n\n" +
                    "Som belöning får du en faktoid om hjärnan! :)\n\n" +
                    "<i><color=orange>\"" + factoids[UnityEngine.Random.Range(0, factoids.Length)] + "\"</color></i>",
                    "Okej!",
                    () => SetupNewQuestion(),
                    "Coolt!",
                    () => SetupNewQuestion());
            else
                SetupNewQuestion();
        }
        else
            PopupController.GetInstance.Open(
                "Wow! Du svarade rätt på alla delar!\n\n" +
                "Du kan mycket om hjärnans delar nu, men kan du verkligen allt om hjärnan?\n\n" +
                "Vill du testa dina kunskaper ytterligare eller starta om det förra spelet?",
                "Testa mer!",
                () => SceneManager.LoadScene("Spel2"),
                "Starta om!",
                () => SceneManager.LoadScene("Spel1"));
    }
    IEnumerator FeedbackWaitTimer(BrainPart part, ButtonPointerHandler handler)
    {
        handler.OnAnswerFeedbackStart(current.CorrectAnswer == part);

        sfx.clip = current.CorrectAnswer == part ? correct : incorrect;
        sfx.Play();

        yield return new WaitForSeconds(1f);

        if (current.CorrectAnswer == part)
            available.Remove(current);

        if (available.Count > 0)
        {
            if (current.CorrectAnswer == part)
                PopupController.GetInstance.Open(
                    "Du hade rätt!\n\n" +
                    "Som belöning får du en faktoid om hjärnan! :)\n\n" +
                    "<i><color=orange>\"" + factoids[UnityEngine.Random.Range(0, factoids.Length)] + "\"</color></i>",
                    "Okej!",
                    () => SetupNewQuestion(),
                    "Coolt!",
                    () => SetupNewQuestion());
            else
                SetupNewQuestion();
        }
        else
        {
            if (SceneManager.GetActiveScene().name == "Spel1")
                PopupController.GetInstance.Open(
                    "Wow! Du svarade rätt på alla delar!\n\n" +
                    "Du kan mycket om hjärnans delar nu, men kan du verkligen allt om hjärnan?\n\n" +
                    "Vill du testa dina kunskaper ytterligare eller starta om det förra spelet?",
                    "Testa mer!",
                    () => SceneManager.LoadScene("Spel2"),
                    "Starta om!",
                    () => SceneManager.LoadScene("Spel1"));
            else
                PopupController.GetInstance.Open(
                    "Wow, nu kan du allt om hjärnan!\n\n" +
                    "Spela hela spelet igen?",
                    "Starta om från början!",
                    () => SceneManager.LoadScene("Spel1"),
                    "",
                    null);
        }
            
    }
}
