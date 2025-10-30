using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel; // assign PausePanel in Inspector
    private bool isPaused = false;

    void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false); // hide panel at start
    }

    void Update()
    {
       
    }

    // Call this from the Pause Button
    public void TogglePause()
    {
        isPaused = !isPaused;

        if (pausePanel != null)
            pausePanel.SetActive(isPaused);

        if (isPaused)
            Time.timeScale = 0f; // freeze game
        else
            Time.timeScale = 1f; // resume game
    }

    // Optional separate Resume button
    public void ResumeGame()
    {
        isPaused = false;
        if (pausePanel != null)
            pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
