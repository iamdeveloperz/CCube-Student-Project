using UnityEngine;
using System.Collections;

public class FlickeringLight : MonoBehaviour
{
    public Light targetLight;

    [Header("±ôºýÀÓ ¼³Á¤")]
    public float minIntensity = 0.3f;
    public float maxIntensity = 1.5f;

    public float minDelay = 0.03f;
    public float maxDelay = 0.15f;

    [Header("°¡²û ±æ°Ô ²¨Áö±â")]
    [Range(0f, 1f)]
    public float blackoutChance = 0.1f;

    public float blackoutMinTime = 0.1f;
    public float blackoutMaxTime = 0.5f;

    private float originalIntensity;

    void Start()
    {
        if (targetLight == null)
            targetLight = GetComponent<Light>();

        originalIntensity = targetLight.intensity;

        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            yield return new WaitForSeconds(
                Random.Range(minDelay, maxDelay)
            );

            // °¡²û ¿ÏÀüÈ÷ ²¨Áü
            if (Random.value < blackoutChance)
            {
                targetLight.enabled = false;

                yield return new WaitForSeconds(
                    Random.Range(blackoutMinTime, blackoutMaxTime)
                );

                targetLight.enabled = true;
            }

            // ¹à±â ·£´ý º¯È­
            targetLight.intensity = Random.Range(
                minIntensity,
                maxIntensity
            );
        }
    }
}