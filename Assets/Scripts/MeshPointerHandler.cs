using UnityEngine;

/// <summary>
/// Klass som byter material på en meshrenderer.
/// Axel Gustafsson, axgu8924
/// </summary>
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

    //något dåliga metodnamn här, borde antagligen vara "Highlight()" och "Reset()"
    public void OnEnter()
    {
        renderer.material = highlightMaterial;
    }
    public void OnExit()
    {
        renderer.material = original;
    }
}
