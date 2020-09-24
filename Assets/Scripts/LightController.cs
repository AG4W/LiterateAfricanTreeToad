using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField]Light[] lights;

    float flickerLength = .1f;
    float flickerTimer = 0f;
    
    float[] targetIntensities;
    float[] originalIntensities;

    int index;

    void Start()
    {
        targetIntensities = new float[lights.Length];
        originalIntensities = new float[lights.Length];

        for (int i = 0; i < targetIntensities.Length; i++)
        {
            targetIntensities[i] = lights[i].intensity;
            originalIntensities[i] = lights[i].intensity;
        }

        flickerLength = Random.Range(.05f, .2f);
        flickerTimer = 0f;
    }
    void Update()
    {
        flickerTimer += Time.deltaTime;

        if (flickerTimer >= flickerLength)
        {
            index = Random.Range(0, lights.Length);

            flickerLength = Random.Range(.05f, .2f);
            flickerTimer = 0f;

            targetIntensities[index] = (Random.Range(0, 1 + 1) == 0) ? 1f : originalIntensities[index];
        }

        for (int i = 0; i < lights.Length; i++)
            lights[i].intensity = Mathf.Lerp(lights[i].intensity, targetIntensities[i], Time.deltaTime * 5f);
    }
}
