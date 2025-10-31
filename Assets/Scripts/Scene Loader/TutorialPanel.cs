using UnityEngine;
using UnityEngine.InputSystem;
public class TutorialPanel : MonoBehaviour
{
    [Header("Tutorial Panel UI")]
    public GameObject tutorialPanel; // Assign your UI panel in the Inspector

    private bool isVisible = true;

    void Start()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true); // Show panel
            isVisible = true;

            // Freeze the game
            Time.timeScale = 0f;
        }
    }

    void Update()
    {
        if (tutorialPanel == null) return;

        // Detect C key using new Input System
        if (isVisible && Keyboard.current.cKey.wasPressedThisFrame)
        {
            // Hide panel
            tutorialPanel.SetActive(false);
            isVisible = false;

            // Unfreeze the game
            Time.timeScale = 1f;
        }
    }
}
