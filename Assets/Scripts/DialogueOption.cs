using UnityEngine;

/// <summary>
/// Klass som används för att definiera en fråga associerat med en modell och ett korrekt svar
/// </summary>
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

/// <summary>
/// Enum som används som svar
/// </summary>
public enum BrainPart
{
    TheFrontalLobe,
    TheParietalLobe,
    TheOccipitalLobe,
    TheTemporalLobe,
    TheCerebellum,
    TheSpinalCord,
}