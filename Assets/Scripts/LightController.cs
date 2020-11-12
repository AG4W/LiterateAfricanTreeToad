using UnityEngine;

/// <summary>
/// Styr de blinkande ljusen i scenen
/// Axel Gustafsson, axgu8924
/// </summary>
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

        //spara originalvärden i originalintensities
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

        //simpel timer
        if (flickerTimer >= flickerLength)
        {
            index = Random.Range(0, lights.Length);

            flickerLength = Random.Range(.05f, .2f);
            flickerTimer = 0f;

            //uppdatera targetIntensities med slumpmässigt värde eller originalvärdet beroende på en coinflip
            targetIntensities[index] = (Random.Range(0, 1 + 1) == 0) ? 1f : originalIntensities[index];
        }

        //interpolera intensiteten istället för att skriva över direkt - detta ger en mjukare och mycket behagligare övergång mellan ljusvärden
        for (int i = 0; i < lights.Length; i++)
            lights[i].intensity = Mathf.Lerp(lights[i].intensity, targetIntensities[i], Time.deltaTime * 5f);
    }
}
