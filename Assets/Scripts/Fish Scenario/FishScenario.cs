using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class FishScenario : MonoBehaviour
{

    [Header("Challenge Settings")]
    public int requiredPresses = 10;        // How many times player must press E
    public float timeLimit = 5f;            // Seconds to complete challenge
    public float interactionRange = 3f;     // Distance needed to start challenge

    [Header("UI Elements")]
    public Slider progressBar;              // Slider showing progress
    public TextMeshProUGUI timerText;       // Timer text display
    public Image textBubble;                // Dialogue bubble image

    [Header("Particles & Audio")]
    public ParticleSystem successParticles;
    public ParticleSystem failParticles;
    public AudioClip successSound;
    public AudioClip failSound;
    private AudioSource audioSource;

    [Header("Dialogue")]
    [TextArea]
    public string dialogueText = "Please help me! I'm stuck in this bag!";
    public TextMeshProUGUI dialogueTMP;     // Text component for dialogue

    private int currentPresses = 0;
    private bool challengeActive = false;
    private float remainingTime;
    private bool completed = false;

    private GameObject player;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");

        // Hide all UI initially
        progressBar.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        textBubble.gameObject.SetActive(false);
        dialogueTMP.gameObject.SetActive(false);
    }

    void Update()
    {
        if (completed) return;
        if (player == null) return;

        float distance = Vector3.Distance(player.transform.position, transform.position);

        // Start challenge when player gets close enough
        if (!challengeActive && distance <= interactionRange)
        {
            StartChallenge();
        }

        if (!challengeActive) return;

        // Countdown logic
        remainingTime -= Time.deltaTime;
        timerText.text = Mathf.Ceil(remainingTime).ToString() + "s";

        // Player pressing E
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentPresses++;
            progressBar.value = (float)currentPresses / requiredPresses;

            if (currentPresses >= requiredPresses)
            {
                CompleteChallenge(true);
            }
        }

        // Fail condition
        if (remainingTime <= 0)
        {
            CompleteChallenge(false);
        }
    }

    public void StartChallenge()
    {
        challengeActive = true;
        completed = false;
        remainingTime = timeLimit;
        currentPresses = 0;

        // Show UI
        progressBar.value = 0;
        progressBar.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
        textBubble.gameObject.SetActive(true);
        dialogueTMP.gameObject.SetActive(true);
        dialogueTMP.text = dialogueText;
    }

    void CompleteChallenge(bool success)
    {
        if (completed) return;
        completed = true;
        challengeActive = false;

        // Hide UI
        progressBar.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        textBubble.gameObject.SetActive(false);
        dialogueTMP.gameObject.SetActive(false);

        // Play effects
        if (success)
        {
            if (successParticles != null) successParticles.Play();
            if (audioSource != null && successSound != null) audioSource.PlayOneShot(successSound);
        }
        else
        {
            if (failParticles != null) failParticles.Play();
            if (audioSource != null && failSound != null) audioSource.PlayOneShot(failSound);
        }

        // Destroy after short delay (2s for animation/sound)
        StartCoroutine(RemoveAfterDelay());
    }

    IEnumerator RemoveAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    // Optional: expose completion state for ScenarioManager
    public bool IsCompleted => completed;
}
