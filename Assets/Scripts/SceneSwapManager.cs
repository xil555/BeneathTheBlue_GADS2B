using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapManager : MonoBehaviour
{
    public static SceneSwapManager instance;

    private DoorTriggerInteraction.DoorToSpawnAt _doorToSpawnTo;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            transform.SetParent(null); // <-- Detach from parent before persisting
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Prevent memory leaks
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find the player
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError($"[SceneSwapManager] Player with tag 'Player' not found in scene: {scene.name}");
            SceneFadeManager.instance?.StartFadeIn(); // Proceed with fade-in even if player not found
            return;
        }

        // Find the matching door spawn point
        var doors = Object.FindObjectsByType<DoorTriggerInteraction>(FindObjectsSortMode.None);
        bool doorFound = false;
        foreach (var door in doors)
        {
            if (door.CurrentDoorPosition == _doorToSpawnTo)
            {
                player.transform.position = door.transform.position;
                Debug.Log($"[SceneSwapManager] Player spawned at door '{_doorToSpawnTo}' in scene: {scene.name}");
                doorFound = true;
                break;
            }
        }

        if (!doorFound)
        {
            Debug.LogWarning($"[SceneSwapManager] No door with DoorToSpawnAt '{_doorToSpawnTo}' found in scene: {scene.name}. Player position unchanged.");
        }

        // Start fade-in
        if (SceneFadeManager.instance != null)
        {
            SceneFadeManager.instance.StartFadeIn();
        }
        else
        {
            Debug.LogError("[SceneSwapManager] SceneFadeManager instance is null!");
        }
    }

    public static void SwapSceneFromDoorUse(SceneField scene, DoorTriggerInteraction.DoorToSpawnAt doorToSpawn)
    {
        if (instance == null)
        {
            Debug.LogError("[SceneSwapManager] SceneSwapManager instance is null!");
            return;
        }
        instance.StartCoroutine(instance.FadeOutThenChangeScene(scene, doorToSpawn));
    }

    private IEnumerator FadeOutThenChangeScene(SceneField myScene, DoorTriggerInteraction.DoorToSpawnAt doorToSpawnAt = DoorTriggerInteraction.DoorToSpawnAt.None)
    {
        Debug.Log("[SceneSwapManager] Starting fade out...");
        if (SceneFadeManager.instance != null)
        {
            SceneFadeManager.instance.StartFadeOut();
        }
        else
        {
            Debug.LogError("[SceneSwapManager] SceneFadeManager instance is null! Skipping fade-out.");
        }

        // Wait until fade out finishes
        while (SceneFadeManager.instance != null && SceneFadeManager.instance.IsFadingOut)
        {
            yield return null;
        }

        Debug.Log($"[SceneSwapManager] Loading scene: {myScene.SceneName}");
        _doorToSpawnTo = doorToSpawnAt;
        SceneManager.LoadScene(myScene.SceneName);
    }
}