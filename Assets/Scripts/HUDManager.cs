using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    [Header("UI Elements")]
    public TMP_Text healthText;
    public TMP_Text coinsText;

    [Header("Game Stats")]
    public int maxHealth = 3;
    private int currentHealth;

    public int totalCoins = 10;
    private int collectedCoins = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHUD();
    }

    void Update()
    {
        //выход в главное меню на ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    public void TakeDamage(int damage = 1)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        UpdateHUD();

        if (currentHealth <= 0)
        {
            // Перезапуск уровня при потере всех сердечек
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void AddCoin(int amount = 1)
    {
        collectedCoins += amount;
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        //Обновление сердечеек
        if (healthText != null)
        {
            string hearts = "";
            for (int i = 0; i < currentHealth; i++)
            {
                hearts += "♥ ";
            }
            healthText.text = hearts.Trim();
        }

        //Счётчик монет
        if (coinsText != null)
        {
            coinsText.text = $"Счет: {collectedCoins} / {totalCoins}";
        }
    }
}