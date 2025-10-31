using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; //  Use new Input System


public class FishScenario : MonoBehaviour
{
    [Header("Challenge Settings")]
    public int requiredPresses = 10;
    public float timeLimit = 5f;
    public float interactionRange = 3f;

    [Header("UI Elements")]
    public Slider progressBar;
    public TextMeshProUGUI timerText;
    public Image textBubble;
    public TextMeshProUGUI dialogueTMP;

    [Header("Particles & Audio")]
    public ParticleSystem successParticles;
    public ParticleSystem failParticles;
    public AudioClip successSound;
    public AudioClip failSound;
    private AudioSource audioSource;

    [Header("Dialogue")]
    [TextArea]
    public string dialogueText = "Please help me! I'm stuck in this bag!";

    private int currentPresses = 0;
    private bool challengeActive = false;
    private float remainingTime;
    private bool completed = false;
    private GameObject player;

    // ? Input System reference
    private InputAction interactAction;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");

        // Hide all UI initially
        progressBar.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        textBubble.gameObject.SetActive(false);
        dialogueTMP.gameObject.SetActive(false);

        // ? Setup Input System
        if (player != null)
        {
            var playerInput = player.GetComponent<PlayerInput>();
            if (playerInput != null)
                interactAction = playerInput.actions["Interact"]; // use your "Interact" action name
        }
    }

    void OnEnable()
    {
        if (interactAction != null)
            interactAction.performed += OnInteract;
    }

    void OnDisable()
    {
        if (interactAction != null)
            interactAction.performed -= OnInteract;
    }

    void Update()
    {
        if (completed || player == null) return;

        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (!challengeActive && distance <= interactionRange)
            StartChallenge();

        if (!challengeActive) return;

        remainingTime -= Time.deltaTime;
        timerText.text = Mathf.Ceil(remainingTime).ToString() + "s";

        if (remainingTime <= 0)
            CompleteChallenge(false);
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!challengeActive) return;

        currentPresses++;
        progressBar.value = (float)currentPresses / requiredPresses;

        if (currentPresses >= requiredPresses)
            CompleteChallenge(true);
    }

    public void StartChallenge()
    {
        challengeActive = true;
        completed = false;
        remainingTime = timeLimit;
        currentPresses = 0;

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

        progressBar.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        textBubble.gameObject.SetActive(false);
        dialogueTMP.gameObject.SetActive(false);

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

        StartCoroutine(RemoveAfterDelay());
    }

    IEnumerator RemoveAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    public bool IsCompleted => completed;
}
