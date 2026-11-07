using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public GameObject staticImage;
    public AudioSource deathSound;

    public PlayerMovement playerMovement;
    public MouseLook mouseLook;
    public Rigidbody rb;

    [Header("생존 시간")]
    public SurvivalTime survivalTime;

    [Header("게임오버 UI")]
    public GameOverUI gameOverUI;

    bool isDead = false;

    private void Start()
    {
        staticImage.SetActive(false);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        // 생존 시간 종료 + 최고 기록 저장
        if (survivalTime != null)
        {
            survivalTime.PlayerCaught();
        }

        // 플레이어 조작 중지
        playerMovement.enabled = false;
        mouseLook.enabled = false;

        rb.velocity = Vector3.zero;
        rb.isKinematic = true;

        // 사망 화면 효과
        staticImage.SetActive(true);

        // 사망 소리
        deathSound.Play();

        // 3초 후 게임오버 UI 표시
        if (gameOverUI != null)
        {
            gameOverUI.ShowGameOver();
        }
    }
}