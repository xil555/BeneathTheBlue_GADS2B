using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // ? for loading scenes
using System.Collections;

public class staminaSystem : MonoBehaviour
{
    public static staminaSystem Instance;

    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float drainRate = 5f;
    public float regenAmount = 25f;

    [Header("UI")]
    public Image staminaBar; // Assign your UI bar image

    [Header("Pickup Settings")]
    public float pickupRespawnTime = 4f;

    [HideInInspector]
    public float currentStamina;

    private bool isDead = false; // prevents multiple scene loads
    private bool staminaActive = false; // true only in Top Zone 1 & 2

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (currentStamina == 0)
            currentStamina = maxStamina;

        UpdateBar();
        CheckScene(SceneManager.GetActiveScene().name);
    }

    void Update()
    {
        if (staminaActive)
            DrainStamina();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckScene(scene.name);
    }

    void CheckScene(string sceneName)
    {
        // Enable stamina system only in Top Zone 1 and 2
        if (sceneName == "Top Zone 1" || sceneName == "Top Zone 2")
        {
            staminaActive = true;
            isDead = false; // reset for new scene
        }
        else
        {
            staminaActive = false;
        }
    }

    void DrainStamina()
    {
        if (currentStamina > 0)
        {
            currentStamina -= drainRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            UpdateBar();

            if (currentStamina <= 0 && !isDead)
            {
                isDead = true;
                StartCoroutine(LoadLoseScreen());
            }
        }
    }

    void UpdateBar()
    {
        if (staminaBar != null)
            staminaBar.fillAmount = currentStamina / maxStamina;
    }

    public void AddStamina(float amount)
    {
        currentStamina += amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        UpdateBar();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!staminaActive) return;

        if (other.CompareTag("StaminaPickup"))
        {
            AddStamina(regenAmount);
            other.gameObject.SetActive(false);
            StartCoroutine(RespawnPickup(other.gameObject, pickupRespawnTime));
        }
    }

    private IEnumerator RespawnPickup(GameObject pickup, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (pickup != null)
            pickup.SetActive(true);
    }

    private IEnumerator LoadLoseScreen()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Lose Screen");
    }

}
