using UnityEngine;
using System.Collections;

public class StaminaPickup2D : MonoBehaviour
{
    public float pickupAmount = 25f;
    public int scoreValue = 10; // How much score this pickup gives

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Add stamina
            if (staminaSystem.Instance != null)
                staminaSystem.Instance.AddStamina(pickupAmount);

            // Add score
            if (ScoreManager.Instance != null)
                ScoreManager.Instance.AddScore(scoreValue);

            // Hide and respawn (optional)
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(4f);
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }
}
