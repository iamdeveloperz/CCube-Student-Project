using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;

    public GameObject startPanel;
    public GameObject gameOverPanel;
    public TMP_Text gameOverText;

    public GameObject crosshair;
    public TMP_Text helpText;

    // 시작 전에는 꺼 두었다가 시작할 때 켬
    public ZombieSpawner zombieSpawner;

    private bool isStarted = false;
    private bool isGameOver = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // 시작 화면에서는 게임 정지
        startPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        Time.timeScale = 0f;
    }

    void Update()
    {
        // SPACE로 게임 시작
        if (!isStarted && Input.GetKeyDown(KeyCode.Space))
        {
            isStarted = true;
            startPanel.SetActive(false);
            Time.timeScale = 1f;
            zombieSpawner.enabled = true;
        }

        // 게임 오버 후 R로 다시 시작
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // 1인칭일 때만 조준점 표시
        bool fps = CameraSwitch.Instance.IsFPS;
        crosshair.SetActive(fps && isStarted && !isGameOver);

        if (fps)
            helpText.text = "[WASD] Move   [Left Click] Shoot   [V] Build Mode";
        else
            helpText.text = "[Left Click] Place Turret (50 Gold)   [V] Shoot Mode";
    }

    // BaseManager.GameOver()에서 호출
    public void ShowGameOver()
    {
        if (isGameOver)
            return;

        isGameOver = true;
        Time.timeScale = 0f;

        gameOverText.text = "Wave : " + WaveManager.instance.wave
            + "\nBest Wave : " + PlayerPrefs.GetInt("BestWave", 0);
        gameOverPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
