using UnityEngine;
using UnityEngine.UI;

public class SurvivalTime : MonoBehaviour
{
    [Header("UI")]
    public Text survivalTimeText;
    public Text bestTimeText;

    private float survivalTime = 0f;
    private float bestTime = 0f;

    private bool beingChased = false;
    private bool gameOver = false;

    void Start()
    {
        bestTime = PlayerPrefs.GetFloat("BestSurvivalTime", 0f);

        UpdateUI();
    }

    void Update()
    {
        if (beingChased && !gameOver)
        {
            survivalTime += Time.deltaTime;
            UpdateUI();
        }
    }

    // 괴물이 플레이어를 처음 발견했을 때
    public void StartChase()
    {
        if (gameOver)
            return;

        beingChased = true;

        Debug.Log("괴물이 플레이어를 발견했습니다!");
    }

    // 괴물에게 잡혔을 때
    public void PlayerCaught()
    {
        if (gameOver)
            return;

        gameOver = true;
        beingChased = false;

        // 최고 기록 갱신
        if (survivalTime > bestTime)
        {
            bestTime = survivalTime;

            PlayerPrefs.SetFloat(
                "BestSurvivalTime",
                bestTime
            );

            PlayerPrefs.Save();

            Debug.Log("최고 기록 갱신!");
        }

        Debug.Log(
            "생존 시간: " +
            survivalTime.ToString("F2") +
            "초"
        );

        UpdateUI();
    }

    void UpdateUI()
    {
        if (survivalTimeText != null)
        {
            survivalTimeText.text =
                "SURVIVAL: " +
                survivalTime.ToString("F2") +
                "s";
        }

        if (bestTimeText != null)
        {
            bestTimeText.text =
                "BEST: " +
                bestTime.ToString("F2") +
                "s";
        }
    }

    public void ResetTime()
    {
        survivalTime = 0f;
        beingChased = false;
        gameOver = false;

        UpdateUI();
    }
}