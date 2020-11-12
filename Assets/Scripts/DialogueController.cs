using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

/// <summary>
/// Dialogkontrollern, detta är klassen som i stort sett sköter hela vårt spel
/// </summary>
public class DialogueController : MonoBehaviour
{
    //ljudkälla i spelet + de effekter vi behöver
    [SerializeField]AudioSource sfx;
    [SerializeField]AudioClip correct;
    [SerializeField]AudioClip incorrect;
    [SerializeField]AudioClip tick;

    //tomt surrogatobjekt som håller alla hjärndelar
    [SerializeField]Transform brainRoot;

    //tomt listobjekt som håller svaren på våra frågor
    [SerializeField]Transform answerList;
    //prefab för svarsknappar
    [SerializeField]GameObject buttonPrefab;

    //textblurben där frågan ställs
    [SerializeField]Text blurb;

    //alla frågor som ska laddas in i programmet
    [SerializeField]DialogueOption[] questions;

    //belöningsfaktoiderna
    [SerializeField]string[] factoids;

    DialogueOption current;
    List<DialogueOption> available;

    void Start()
    {
        //skapa lista ifrån alla möjliga frågor
        available = new List<DialogueOption>(questions);

        //starta spelet
        SetupNewQuestion();
    }
    void SetupNewQuestion()
    {
        //välj en slumpmässig fråga
        current = available[UnityEngine.Random.Range(0, available.Count)];

        UpdateUI();
    }

    void UpdateUI()
    {
        //ta bort gamla frågor, svar och modeller
        for (int i = 0; i < answerList.childCount; i++)
            Destroy(answerList.GetChild(i).gameObject);
        for (int i = 0; i < brainRoot.childCount; i++)
            Destroy(brainRoot.GetChild(i).gameObject);

        //spawna modeller ifall vi är i spel 1
        if(SceneManager.GetActiveScene().name == "Spel1")
        {
            for (int i = 0; i < available.Count; i++)
            {
                DialogueOption option = available[i];

                //skapa knapp och modell i scenen
                GameObject g = Instantiate(buttonPrefab, answerList);
                GameObject p = Instantiate(option.CorrectBrainPart, brainRoot);
                MeshPointerHandler mph = p.GetComponentInChildren<MeshPointerHandler>();
                ButtonPointerHandler bph = g.transform.GetComponentInChildren<ButtonPointerHandler>();

                //sätt knapptexten till svaret
                g.transform.Find("Text").GetComponentInChildren<Text>().text = available[i].name;
                //registrera event för ljudeffekt + highlight + klick
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
            //i spel 2 använder vi ett enum då vi ej använder modeller, fungerar i övrigt likadant
            for (int i = 0; i < Enum.GetNames(typeof(BrainPart)).Length; i++)
            {
                BrainPart bp = (BrainPart)i;

                GameObject g = Instantiate(buttonPrefab, answerList);

                ButtonPointerHandler bph = g.transform.GetComponentInChildren<ButtonPointerHandler>();

                //lägg till ett tomrum mellan varje stor bokstav i enumet och casta till sträng för att det ska se bättre ut i svaren "TheFrontalLobe" >> "The Frontal Lobe"
                g.transform.Find("Text").GetComponentInChildren<Text>().text = Regex.Replace(bp.ToString(), "[A-Z]", " $0");
                bph.OnEnter += (ButtonPointerHandler h) => sfx.PlayOneShot(tick);
                bph.OnClick += (ButtonPointerHandler h) => OnClick(bp, bph);
            }
        }

        //sätt blurbtext
        blurb.text = "<size=24><color=orange>Which part of the brain is this?</color></size>\n\n" + current.Description;

        //stäng av modellen ifall vi är i spel 2
        if (SceneManager.GetActiveScene().name == "Spel2")
            brainRoot.gameObject.SetActive(false);
    }

    //Vi kan använda oss av overloading för att undvika att ha separata controllers för varje spel, desssa wrapper till feedback som behöver timerfunktionalitet
    void OnClick(DialogueOption option, ButtonPointerHandler handler) => this.StartCoroutine(FeedbackWaitTimer(option, handler));
    void OnClick(BrainPart part, ButtonPointerHandler handler) => this.StartCoroutine(FeedbackWaitTimer(part, handler));

    //den enda skillnaden mellan dessa metoder är hur frågan utvärderas, i spel 1 så sker en check mot rätt modell
    //medans i spel två sker en check mot ett enum
    IEnumerator FeedbackWaitTimer(DialogueOption option, ButtonPointerHandler handler)
    {
        handler.OnAnswerFeedbackStart(option.CorrectBrainPart == current.CorrectBrainPart);

        sfx.clip = option.CorrectBrainPart == current.CorrectBrainPart ? correct : incorrect;
        sfx.Play();

        //vänta 1 sekund medans feedback animeras
        yield return new WaitForSeconds(1f);

        //ta bort frågan ur frågepoolen ifall svaret var korrekt
        if (option.CorrectBrainPart == current.CorrectBrainPart)
            available.Remove(current);

        //om det fortfarande finns frågor
        if (available.Count > 0)
        {
            //om svaret var korrekt, visa belöningsfaktoid
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

        //vänta 1 sekund medans feedback animeras
        yield return new WaitForSeconds(1f);

        //ta bort frågan ur frågepoolen ifall svaret var korrekt
        if (current.CorrectAnswer == part)
            available.Remove(current);

        //om det fortfarande finns frågor
        if (available.Count > 0)
        {
            //om svaret var korrekt, visa belöningsfaktoid
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
            //vi ska ha olika popupsettings beroende på vilket spel vi är i.
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
