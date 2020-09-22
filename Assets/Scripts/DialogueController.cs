using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DialogueController : MonoBehaviour
{
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

            g.transform.Find("Text").GetComponentInChildren<Text>().text = available[i].name;
            g.transform.GetComponentInChildren<ButtonPointerHandler>().OnEnter += (ButtonPointerHandler h) => mph.OnEnter();
            g.transform.GetComponentInChildren<ButtonPointerHandler>().OnClick += (ButtonPointerHandler h) => {
                mph.OnExit();
                OnClick(option);
            };
            g.transform.GetComponentInChildren<ButtonPointerHandler>().OnExit += (ButtonPointerHandler h) => mph.OnExit();
        }

        blurb.text = current.Description;
    }
    void OnClick(DialogueOption option)
    {
        Debug.Log("on click");

        if (option.CorrectBrainPart == current.CorrectBrainPart)
        {
            Debug.Log("correct");

            available.Remove(current);
        }
        else
        {
            Debug.Log("wrong");
        }

        if (available.Count > 0)
            SetupNewQuestion();
        else
            SceneManager.LoadScene("SampleScene");
    }
}
