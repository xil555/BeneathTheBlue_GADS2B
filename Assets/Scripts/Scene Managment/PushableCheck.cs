using UnityEngine;

public class PushableCheck : MonoBehaviour
{
    private void Start()
    {
        if (GameManager.Instance != null &&
            GameManager.Instance.destroyedPushables.Contains(gameObject.name))
        {
            Destroy(gameObject);
        }
    }
}
