using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;

    [Header("Настройки движения")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f; // Сила физического прыжка

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Находим Animator на дочернем объекте (на самой 3D-модели)
        animator = GetComponentInChildren<Animator>();

        // Запрещаем объекту вращаться/падать от физических столкновений
        if (rb != null)
        {
            rb.freezeRotation = true;
        }
    }

    void Update()
    {
        // 1. Считываем ввод с клавиш (W/A/S/D / Стрелочки)
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = new Vector3(moveX, 0f, moveZ).normalized;

        // 2. Движение с учетом поворота персонажа
        Vector3 movement = transform.TransformDirection(moveDir);

        if (rb != null)
        {
            // Используем rb.velocity (или rb.linearVelocity в Unity 6)
            // Двигаем по X и Z, оставляя текущую гравитацию Y
            rb.linearVelocity = new Vector3(movement.x * moveSpeed, rb.linearVelocity.y, movement.z * moveSpeed);
        }

        if (animator != null)
        {
            // 3. Анимация ходьбы
            bool run = moveDir.magnitude > 0.1f;
            animator.SetBool("run", run);

            // 4. Прыжок по Пробелу (ИСПРАВЛЕНО: "Jump" вместо "Jumping")
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Вызываем триггер "Jump", который у тебя создан в Animator
                animator.SetTrigger("Jump");

                if (rb != null)
                {
                    // Сбрасываем Y-скорость перед прыжком для стабильной высоты
                    rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                }
            }

            // 5. Смерть по кнопке K (убедись, что в Animator создан Trigger "IsDead")
            if (Input.GetKeyDown(KeyCode.K))
            {
                animator.SetTrigger("IsDead");
            }
        }
    }
}