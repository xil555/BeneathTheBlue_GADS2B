using UnityEngine;
using UnityEngine.SceneManagement;
public class DestroyToWin : MonoBehaviour
{
    [Header("The specific object to check")]
    public GameObject targetObject;

    [Header("Scene to load when object is destroyed")]
    public string winSceneName = "Win Scene";

    private bool hasLoaded = false;

    void Update()
    {
        // If target has already been destroyed
        if (!hasLoaded && targetObject == null)
        {
            hasLoaded = true;
            Debug.Log("🎉 Target object destroyed! Loading Win Scene...");
            SceneManager.LoadScene(winSceneName);
        }
    }
}
