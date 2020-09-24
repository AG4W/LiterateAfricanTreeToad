using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
        current = available[Random.Range(0, available.Count)];

        UpdateUI();
    }

    void UpdateUI()
    {
        for (int i = 0; i < answerList.childCount; i++)
            Destroy(answerList.GetChild(i).gameObject);
        for (int i = 0; i < brainRoot.childCount; i++)
            Destroy(brainRoot.GetChild(i).gameObject);

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

        blurb.text = current.Description;
    }
    void OnClick(DialogueOption option, ButtonPointerHandler handler) => this.StartCoroutine(FeedbackWaitTimer(option, handler));

    IEnumerator FeedbackWaitTimer(DialogueOption option, ButtonPointerHandler handler)
    {
        handler.OnAnswerFeedbackStart(option.CorrectBrainPart == current.CorrectBrainPart);

        sfx.clip = option.CorrectBrainPart == current.CorrectBrainPart ? correct : incorrect;
        sfx.Play();

        yield return new WaitForSeconds(1f);

        if (option.CorrectBrainPart == current.CorrectBrainPart)
            available.Remove(current);

        if (available.Count > 0)
            SetupNewQuestion();
        else
            SceneManager.LoadScene("SampleScene");
    }
}
