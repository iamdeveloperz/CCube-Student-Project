using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DeathStaticEffect : MonoBehaviour
{
    [Header("지지직 이미지")]
    public Image staticImage;

    [Header("효과 시간")]
    public float duration = 2f;

    [Header("깜빡이는 속도")]
    public float flickerSpeed = 0.03f;

    [Header("최대 투명도")]
    [Range(0, 1)]
    public float maxAlpha = 1f;

    public void PlayEffect()
    {
        gameObject.SetActive(true);
        StartCoroutine(StaticRoutine());
    }

    IEnumerator StaticRoutine()
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += flickerSpeed;

            Color c = staticImage.color;
            c.a = Random.Range(0.3f, maxAlpha);
            staticImage.color = c;

            staticImage.rectTransform.localRotation =
                Quaternion.Euler(0, 0, Random.Range(-2f, 2f));

            yield return new WaitForSeconds(flickerSpeed);
        }

        Color end = staticImage.color;
        end.a = 1f;
        staticImage.color = end;
    }
}