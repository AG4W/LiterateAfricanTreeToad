using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]float speed;

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
            this.transform.eulerAngles += new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0f) * speed * Time.deltaTime;
    }
}
