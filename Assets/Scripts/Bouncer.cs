using UnityEngine;

public class Bouncer : MonoBehaviour
{
    public float bounceForce = 500f;

    private void OnCollisionEnter(Collision collision)
    {
        // Проверяем наличие скрипта игрока
        if (collision.gameObject.TryGetComponent<PlayerMovement>(out var player))
        {
            // Получаем Rigidbody игрока и подбрасываем вверх
            if (collision.gameObject.TryGetComponent<Rigidbody>(out var rb))
            {
                // Сбрасываем Y-скорость, чтобы подброс всегда был одинаковой высоты
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
                rb.AddForce(Vector3.up * bounceForce);
            }
        }
    }
}