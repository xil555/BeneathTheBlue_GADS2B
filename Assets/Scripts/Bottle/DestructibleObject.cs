using UnityEngine;
using UnityEngine.SceneManagement;

public class DestructibleObject : MonoBehaviour
{
    public int scoreValue = 1; // points per bottle

    public void Hit()
    {
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddScore(scoreValue);

        gameObject.SetActive(false); // hide the bottle
    }

}
