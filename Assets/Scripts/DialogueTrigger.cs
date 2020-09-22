using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField]Material corrrectAnswerMaterial;
    [SerializeField]Material incorrectAnswerMaterial;

    Material original;

    MeshRenderer renderer;

    void Start()
    {
        renderer = this.GetComponent<MeshRenderer>();
        original = renderer.sharedMaterial;
    }

    public delegate void OnClickEvent(DialogueTrigger trigger);
    public static OnClickEvent OnAnswerSubmitted;
}
