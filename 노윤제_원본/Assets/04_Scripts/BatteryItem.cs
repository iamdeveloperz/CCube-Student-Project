using UnityEngine;
using System.Collections;

public class BatteryItem : MonoBehaviour
{
    [Header("배터리 회복량")]
    public float batteryAmount = 25f;

    [Header("자동 삭제")]
    public float lifeTime = 10f;

    [Header("깜빡임")]
    public float flickerTime = 2f;
    public float flickerSpeed = 0.1f;

    private bool collected = false;

    private Renderer[] renderers;
    private Collider itemCollider;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        itemCollider = GetComponent<Collider>();

        StartCoroutine(LifeTimer());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;

        if (other.CompareTag("Player"))
        {
            Flashlight flashlight = other.GetComponentInChildren<Flashlight>();

            if (flashlight != null)
            {
                flashlight.AddBattery(batteryAmount);

                collected = true;

                Destroy(gameObject);
            }
        }
    }

    IEnumerator LifeTimer()
    {
        // 10초 동안 대기
        yield return new WaitForSeconds(lifeTime);

        // 깜빡이기 시작
        if (itemCollider != null)
            itemCollider.enabled = false;

        float timer = 0f;

        while (timer < flickerTime)
        {
            foreach (Renderer r in renderers)
            {
                r.enabled = !r.enabled;
            }

            yield return new WaitForSeconds(flickerSpeed);

            timer += flickerSpeed;
        }

        Destroy(gameObject);
    }
}