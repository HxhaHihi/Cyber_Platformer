using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Настройки")]
    public float rotateSpeed = 100f;

    [Header("Эффекты")]
    public GameObject pickupEffectPrefab; // Ссылка на префаб CoinPickupEffect

    void Update()
    {
        transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. Создаём эффект сбора в точке, где стояла монетка
            if (pickupEffectPrefab != null)
            {
                Instantiate(pickupEffectPrefab, transform.position, Quaternion.identity);
            }

            // Передаём +1 очко в ваш GameManager (если он есть на сцене)
            if (GameManager.instance != null)
            {
                GameManager.instance.AddScore(1);
            }

            // Передаем +1 к счету в наш HUDManager
            if (HUDManager.Instance != null)
            {
                HUDManager.Instance.AddCoin(1);
            }

            Destroy(gameObject);
        }
    }
}