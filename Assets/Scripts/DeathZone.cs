using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public Vector3 respawnPoint = new Vector3(-3.667f, 0.39f, -0.32f);

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        HUDManager.Instance?.TakeDamage(1);

        if (other.attachedRigidbody != null)
        {
            other.attachedRigidbody.linearVelocity = Vector3.zero;
            other.attachedRigidbody.angularVelocity = Vector3.zero;
        }

        other.transform.position = respawnPoint;
    }
}