using UnityEngine;
using UnityEngine.Audio;

public class DamagePlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip suckSound;
    public float damageAmount = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Look for the player using the tag
        if (collision.CompareTag("Player"))
        {
            // Play audio if assigned
            if (audioSource != null && suckSound != null)
                audioSource.PlayOneShot(suckSound);

            playerHealth PlayerHealth = collision.GetComponent<playerHealth>();
            if (PlayerHealth != null)
            {
                PlayerHealth.TakeDamage(damageAmount);
                Debug.Log("Player took damage!");
            }
        }
    }
}
