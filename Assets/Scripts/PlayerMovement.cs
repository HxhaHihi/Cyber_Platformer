using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;

    [Header("Настройки движения")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        Vector3 movement = transform.TransformDirection(input.normalized);
        rb.linearVelocity = new Vector3(movement.x * moveSpeed, rb.linearVelocity.y, movement.z * moveSpeed);

        if (animator != null)
            animator.SetBool("run", input.sqrMagnitude > 0.01f);

        if (!Input.GetKeyDown(KeyCode.Space))
            return;

        animator?.SetTrigger("Jump");
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}