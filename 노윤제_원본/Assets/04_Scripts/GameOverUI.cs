using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [Header("게임오버 패널")]
    public GameObject gameOverPanel;

    public GameObject staticImage;

    [Header("표시 시간")]
    public float showDelay = 3f;

    void Start()
    {
        gameOverPanel.SetActive(false);
    }

    // 플레이어가 죽었을 때 호출
    public void ShowGameOver()
    {
        StartCoroutine(ShowGameOverCoroutine());
    }

    IEnumerator ShowGameOverCoroutine()
    {
        yield return new WaitForSeconds(showDelay);

        gameOverPanel.SetActive(true);

        staticImage.SetActive(false);

        // 마우스 표시
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // 다시 시작 버튼
    public void RestartGame()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    // 종료 버튼
    public void QuitGame()
    {
        Debug.Log("게임 종료");

        Application.Quit();
    }
}