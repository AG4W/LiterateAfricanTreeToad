using UnityEngine;

[CreateAssetMenu(menuName = "Assets/Question")]
public class DialogueOption : ScriptableObject
{
    [SerializeField]GameObject correctBrainPart;
    [TextArea(7, 10)][SerializeField]string description;

    [SerializeField]BrainPart correctAnswer;

    public GameObject CorrectBrainPart => correctBrainPart;
    public string Description => description;

    public BrainPart CorrectAnswer => correctAnswer;
}

public enum BrainPart
{
    TheFrontalLobe,
    TheParietalLobe,
    TheOccipitalLobe,
    TheTemporalLobe,
    TheCerebellum,
    TheSpinalCord,
}