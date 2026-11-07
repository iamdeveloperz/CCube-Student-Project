using UnityEngine;

// 플레이어가 닿으면 죽는 트랩 (가시 함정 / 귀신 몸통 판정에 사용)
// FPSController 의 Die() 를 SendMessage 로 호출하므로 기존 스크립트는 수정하지 않아도 됩니다.
public class DeathTrap : MonoBehaviour
{
    [Header("가시 (비워두면 항상 위험)")]
    public Transform spikes;

    // true 이면 가시가 주기적으로 튀어나왔다 들어갑니다.
    public bool popUp = true;

    [Header("가시 타이밍")]
    public float downTime = 1.8f;   // 가시가 숨어 있는 시간
    public float upTime = 1.6f;     // 가시가 올라와 있는 시간
    public float moveTime = 0.12f;  // 올라오고 내려가는 데 걸리는 시간
    public float hiddenDepth = 0.5f; // 숨을 때 내려가는 깊이
    public float startDelay = 0f;   // 트랩마다 타이밍을 다르게 하기 위한 지연

    [Header("소리 (선택)")]
    public AudioSource audioSource;
    public AudioClip popSound;

    private Vector3 upPosition;
    private float timer;
    private bool dangerous = true;

    void Start()
    {
        if (spikes != null)
            upPosition = spikes.localPosition;

        timer = -startDelay;
    }

    void Update()
    {
        // 가시가 없거나 튀어나오기 기능을 끄면 항상 위험
        if (!popUp || spikes == null)
        {
            dangerous = true;
            return;
        }

        timer += Time.deltaTime;
        float t = Mathf.Repeat(Mathf.Max(timer, 0f), downTime + upTime);

        // height : 0 = 숨음, 1 = 완전히 올라옴
        float height;
        if (t < downTime)
        {
            height = 0f;
        }
        else
        {
            float u = t - downTime;
            if (u < moveTime)
                height = u / moveTime;
            else if (u > upTime - moveTime)
                height = (upTime - u) / moveTime;
            else
                height = 1f;
        }

        spikes.localPosition = upPosition + Vector3.down * hiddenDepth * (1f - height);

        bool wasDangerous = dangerous;
        dangerous = height > 0.6f;

        // 가시가 튀어나오는 순간 소리
        if (dangerous && !wasDangerous && audioSource != null && popSound != null)
            audioSource.PlayOneShot(popSound);
    }

    void OnTriggerEnter(Collider other)
    {
        TryKill(other);
    }

    void OnTriggerStay(Collider other)
    {
        TryKill(other);
    }

    void TryKill(Collider other)
    {
        if (!dangerous)
            return;

        if (!other.CompareTag("Player"))
            return;

        // FPSController 의 Die() 호출 (이미 죽은 상태면 Die 안에서 무시됨)
        other.SendMessage("Die", SendMessageOptions.DontRequireReceiver);
    }
}
