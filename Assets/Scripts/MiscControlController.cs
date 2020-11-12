using UnityEngine;

/// <summary>
/// Redundant klass som aldrig används
/// Axel Gustafsson, axgu8924
/// </summary>
public class MiscControlController : MonoBehaviour
{
    [SerializeField]GameObject restart;

    void Start()
    {
        restart.GetComponent<ButtonPointerHandler>();
    }
}
