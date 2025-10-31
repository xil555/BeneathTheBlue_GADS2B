using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ScenarioManager : MonoBehaviour
{
    [Header("Scenario Settings")]
    public FishScenario[] scenarioPrefabs;      // Goldfish, Dolphin, Shark prefabs
    public Transform[] spawnPoints;             // Where they can spawn
    public float firstScenarioDelay = 10f;      // Delay before first scenario
    public float intervalBetweenScenarios = 10f;// Delay between each scenario

    private int currentScenarioIndex = 0;
    private static HashSet<string> completedScenarios = new HashSet<string>(); // tracks what’s been done

    private void Start()
    {
        StartCoroutine(HandleScenarios());
    }

    IEnumerator HandleScenarios()
    {
        // Wait before spawning the first scenario
        yield return new WaitForSeconds(firstScenarioDelay);

        while (currentScenarioIndex < scenarioPrefabs.Length)
        {
            FishScenario prefab = scenarioPrefabs[currentScenarioIndex];
            string scenarioName = prefab.name;

            // Skip if already completed
            if (completedScenarios.Contains(scenarioName))
            {
                currentScenarioIndex++;
                continue;
            }

            // Spawn and start scenario
            FishScenario scenario = SpawnScenario(prefab);

            // Wait until player completes or fails the scenario
            yield return new WaitUntil(() => scenario == null || scenario.IsCompleted);

            // Mark as done
            completedScenarios.Add(scenarioName);

            // Wait before next one
            yield return new WaitForSeconds(intervalBetweenScenarios);

            currentScenarioIndex++;
        }

        Debug.Log("All scenarios completed.");
    }

    FishScenario SpawnScenario(FishScenario prefab)
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning(" No spawn points assigned!");
            return null;
        }

        // Pick a random spawn location
        Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Instantiate prefab
        FishScenario scenario = Instantiate(prefab, spawn.position, Quaternion.identity);
        scenario.StartChallenge();

        return scenario;
    }
}
