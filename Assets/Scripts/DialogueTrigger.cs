using UnityEngine;

/// <summary>
/// Redudant klass som aldrig används
/// Axel Gustafsson, axgu8924
/// </summary>
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
