using UnityEngine;

[CreateAssetMenu(menuName = "Assets/Question")]
public class DialogueOption : ScriptableObject
{
    [SerializeField]GameObject correctBrainPart;
    [TextArea(7, 10)][SerializeField]string description;

    public GameObject CorrectBrainPart => correctBrainPart;
    public string Description => description;
}
