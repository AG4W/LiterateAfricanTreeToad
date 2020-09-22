using UnityEngine;

public class MeshPointerHandler : MonoBehaviour
{
    [SerializeField]Material highlightMaterial;
    Material original;

    MeshRenderer renderer;

    void Start()
    {
        renderer = this.GetComponent<MeshRenderer>();
        original = renderer.sharedMaterial;
    }

    public void OnEnter()
    {
        renderer.material = highlightMaterial;
    }
    public void OnExit()
    {
        renderer.material = original;
    }
}
