using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadWinOnTouch : MonoBehaviour
{
    [Header("Scene to Load")]
    public string winSceneName = "Win Scene";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("🎉 Player touched the object! Loading Win Scene...");
            SceneManager.LoadScene(winSceneName);
        }
    }
}
