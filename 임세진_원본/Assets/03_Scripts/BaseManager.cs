using UnityEngine;
using TMPro;

public class BaseManager : MonoBehaviour
{
    public static BaseManager instance;

    public int maxHP = 100;
    public int currentHP;

    public TMP_Text hpText;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentHP = maxHP;
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP < 0)
            currentHP = 0;

        UpdateUI();

        if (currentHP <= 0)
        {
            GameOver();
        }
    }

    void UpdateUI()
    {
        if (hpText != null)
            hpText.text = "Base HP : " + currentHP;
    }

    void GameOver()
    {
        Debug.Log("GAME OVER");
    }
}