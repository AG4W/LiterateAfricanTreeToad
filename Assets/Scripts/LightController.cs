using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField]Light[] lights;

    float flickerLength = .1f;
    float flickerTimer = 0f;
    
    Light current;

    void Start()
    {
        current = lights[Random.Range(0, lights.Length)];
        current.gameObject.SetActive(false);

        flickerLength = Random.Range(.05f, .2f);
        flickerTimer = 0f;
    }
    void Update()
    {
        flickerTimer += Time.deltaTime;

        if (flickerTimer >= flickerLength)
        {
            current.gameObject.SetActive(true);
            current = lights[Random.Range(0, lights.Length)];

            flickerLength = Random.Range(.05f, .2f);
            flickerTimer = 0f;

            current.gameObject.SetActive(false);
        }
    }
}
