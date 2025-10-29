using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FinishPoint : MonoBehaviour
{
    [SerializeField] string nextSceneName;
    [SerializeField] string nextSceneSpawnID; // The spawn ID where the player should appear in the next scene

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneController.instance.LoadScene(nextSceneName, nextSceneSpawnID);
        }

        StartCoroutine(ReenableCollider());
    }

    private IEnumerator ReenableCollider()
    {
        yield return new WaitForSeconds(2f);
        GetComponent<Collider2D>().enabled = true;
    }
}
