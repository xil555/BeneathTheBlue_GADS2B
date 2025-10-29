using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    public static SceneController instance;
    private string nextSpawnID;
    private bool isTransitioning = false; // <- lock to prevent glitch loops

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Call this to load a scene
    public void LoadScene(string sceneName, string spawnID = "")
    {
        if (isTransitioning) return; // <- ignore if already transitioning

        isTransitioning = true;
        nextSpawnID = spawnID;
        SceneManager.LoadSceneAsync(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Place the player at the spawn point
        if (!string.IsNullOrEmpty(nextSpawnID))
        {
            SpawnPoint[] spawns = Object.FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
            foreach (var spawn in spawns)
            {
                if (spawn.spawnID == nextSpawnID)
                {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    if (player != null)
                    {
                        player.transform.position = spawn.transform.position;
                    }
                    break;
                }
            }
            nextSpawnID = "";
        }

        isTransitioning = false; // <- unlock transition after scene fully loaded
    }

}

