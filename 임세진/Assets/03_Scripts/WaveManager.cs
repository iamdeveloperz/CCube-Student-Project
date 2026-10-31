using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    public TMP_Text waveText;
    public TMP_Text bestWaveText;

    public int wave = 1;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateUI();
        UpdateBestWaveUI();
    }

    // 다음 웨이브
    public void NextWave()
    {
        wave++;
        UpdateUI();

        Debug.Log("Wave " + wave + " 시작!");
    }

    // 현재 웨이브 UI
    void UpdateUI()
    {
        if (waveText != null)
            waveText.text = "Wave : " + wave;
    }

    // 게임 오버 시 호출
    public void OnGameOver()
    {
        int bestWave = PlayerPrefs.GetInt("BestWave", 0);

        if (wave > bestWave)
        {
            PlayerPrefs.SetInt("BestWave", wave);
            PlayerPrefs.Save();
        }

        UpdateBestWaveUI();
    }

    // 최고 웨이브 UI
    void UpdateBestWaveUI()
    {
        if (bestWaveText != null)
        {
            bestWaveText.text = "Best Wave : " + PlayerPrefs.GetInt("BestWave", 0);
        }
    }

    private void OnApplicationQuit()
    {
        int bestWave = PlayerPrefs.GetInt("BestWave", 0);

        if (wave > bestWave)
        {
            PlayerPrefs.SetInt("BestWave", wave);
            PlayerPrefs.Save();
        }
    }
}