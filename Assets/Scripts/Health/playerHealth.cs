using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playerHealth : MonoBehaviour
{
    public float health = 100f;       // Current health
    private float maxHealth;          // Max health
    private Image healthBar;          // Reference to the health bar image

    public Animator animator;         // Reference to player's Animator
    public string loseSceneName = "LoseScreen"; // Name of the lose scene

    void Awake()
    {
        maxHealth = health;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        FindHealthBar();
        UpdateHealthBar();

        // Try to automatically find the Animator if not assigned
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindHealthBar();
        UpdateHealthBar();
    }

    void FindHealthBar()
    {
        GameObject barObj = GameObject.FindGameObjectWithTag("HealthBar");
        if (barObj != null)
        {
            healthBar = barObj.GetComponent<Image>();
        }
        else
        {
            healthBar = null;
            Debug.LogWarning("No HealthBar found in this scene!");
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        // Play hurt animation if Animator is assigned
        if (animator != null)
        {
            animator.SetTrigger("Hurt");
        }

        UpdateHealthBar();

        // Check if health is zero
        if (health <= 0)
        {
            Lose();
        }
    }

    public void Heal(float amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = health / maxHealth;
        }
    }

    void Lose()
    {
        // Optionally, you could also play a death animation here
        // Then load lose screen
        SceneManager.LoadScene(loseSceneName);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
