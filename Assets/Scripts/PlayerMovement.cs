using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;

    [Header("Настройки")]
    public float moveSpeed = 5f; // Скорость для физического перемещения
    public float jumpForce = 7f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        // 1. Считываем ввод
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 inputDir = new Vector3(moveX, 0f, moveZ);
        float inputMagnitude = Mathf.Clamp01(inputDir.magnitude);

        
        if (animator != null)
        {
            animator.SetFloat("Speed", inputMagnitude);
        }

        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator?.SetTrigger("Jump");
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
       
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 moveDir = new Vector3(moveX, 0f, moveZ).normalized;

        if (moveDir.magnitude > 0.1f)
        {
            
            Vector3 targetPosition = rb.position + transform.TransformDirection(moveDir) * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPosition);
        }
    }
}