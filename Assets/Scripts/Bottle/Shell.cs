using UnityEngine;

public class Shell : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip suckSound;
    public ParticleSystem splashEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pushable"))
        {
            // Play audio if assigned
            if (audioSource != null && suckSound != null)
                audioSource.PlayOneShot(suckSound);

            // Spawn particles
            if (splashEffect != null)
                Instantiate(splashEffect, other.transform.position, Quaternion.identity);

            // Remember this pushable so it doesn't respawn later
            if (GameManager.Instance != null)
                GameManager.Instance.destroyedPushables.Add(other.gameObject.name);

            // Destroy the object
            Destroy(other.gameObject);
        }
    }
}
