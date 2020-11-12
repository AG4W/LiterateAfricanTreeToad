using UnityEngine;

/// <summary>
/// Simpel klass som roterar objektet 'den sitter på'
/// Används i vårt fall för att rotera hjärnobjektet
/// Axel Gustafsson, axgu8924
/// </summary>
public class Rotator : MonoBehaviour
{
    [SerializeField]float speed;

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
            this.transform.eulerAngles += new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0f) * speed * Time.deltaTime;
    }
}
