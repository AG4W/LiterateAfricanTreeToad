using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]float speed;

    void Update()
    {
        Cursor.lockState = Input.GetKey(KeyCode.Mouse0) ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !Input.GetKey(KeyCode.Mouse0);

        if (Input.GetKey(KeyCode.Mouse0))
            this.transform.eulerAngles += new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0f) * speed * Time.deltaTime;
    }
}
