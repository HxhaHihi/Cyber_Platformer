using UnityEngine;
using TMPro; // Обязательно для работы с TextMeshPro

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI Элементы")]
    public TMP_Text scoreText;

    private int score = 0;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "счёт: " + score;
        }
    }
}