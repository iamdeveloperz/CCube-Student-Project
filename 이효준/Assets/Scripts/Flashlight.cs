using UnityEngine;
using System.Collections;

public class Flashlight : MonoBehaviour
{
    [Header("References")]
    public Light flashlight;
    public AudioSource audioSource;
    public AudioClip turnOnSound;
    public AudioClip turnOffSound;

    [Header("Battery")]
    public float battery = 100f;
    public float batteryDrain = 5f;

    [Header("Light Intensity")]
    public float maxIntensity = 4f;
    public float minIntensity = 0.8f;

    private bool isOn = false;
    private bool flickering = false;

    void Start()
    {
        flashlight.enabled = false;
        flashlight.intensity = maxIntensity;
    }

    void Update()
    {
     
        if (Input.GetMouseButtonDown(0))
        {
            if (isOn)
            {
                TurnOff();
            }
            else if (battery > 0)
            {
                TurnOn();
            }
        }

        if (isOn)
        {
            // 배터리 감소
            battery -= batteryDrain * Time.deltaTime;
            battery = Mathf.Clamp(battery, 0f, 100f);

            // 배터리에 따라 밝기 변경
            flashlight.intensity = Mathf.Lerp(
                minIntensity,
                maxIntensity,
                battery / 100f
            );

            // 20% 이하부터 깜빡임
            if (battery <= 20f && !flickering)
            {
                StartCoroutine(Flicker());
            }

            // 배터리 방전
            if (battery <= 0)
            {
                TurnOff();
            }
        }
    }

    void TurnOn()
    {
        isOn = true;
        flashlight.enabled = true;

        if (turnOnSound != null)
            audioSource.PlayOneShot(turnOnSound);
    }

    void TurnOff()
    {
        isOn = false;
        flashlight.enabled = false;

        if (turnOffSound != null)
            audioSource.PlayOneShot(turnOffSound);
    }

    IEnumerator Flicker()
    {
        flickering = true;

        while (isOn && battery > 0 && battery <= 20f)
        {
            flashlight.enabled = false;
            yield return new WaitForSeconds(Random.Range(0.03f, 0.08f));

            flashlight.enabled = true;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.6f));
        }

        flickering = false;
    }
    public void AddBatteryAmount(float amount)
    {
        battery += amount;
        battery = Mathf.Clamp(battery, 0f, 100f);
    }
}