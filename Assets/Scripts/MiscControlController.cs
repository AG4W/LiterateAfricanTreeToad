using UnityEngine;

public class MiscControlController : MonoBehaviour
{
    [SerializeField]GameObject restart;

    void Start()
    {
        restart.GetComponent<ButtonPointerHandler>();
    }
}
