using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public Vector3 respawnPoint = new Vector3(-3.667f, 0.39f, -0.32f);

    private void OnTriggerEnter(Collider other)
    {
        //Именно игрок
        if (other.CompareTag("Player") || other.GetComponent<PlayerMovement>() != null || other.name == "Player")
        {
            // 1. Отнимаем 1 сердечко в HUDManager
            if (HUDManager.Instance != null)
            {
                HUDManager.Instance.TakeDamage(1);
            }

            // 2. Сбрасываем накопившуюся скорость падения, чтобы игрок не летал и не падал сквозь пол
            if (other.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            // 3. Телепортируем на спавн
            other.transform.position = respawnPoint;
        }
    }
}