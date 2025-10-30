using UnityEngine;
using UnityEngine.UI;
public class MusicButtonLinker : MonoBehaviour
{
    public Button onButton;
    public Button offButton;

    void Start()
    {
        if (MusicManager.Instance == null)
        {
            Debug.LogError("No MusicManager found!");
            return;
        }

        if (onButton != null)
            onButton.onClick.AddListener(() => MusicManager.Instance.PlayMusic());

        if (offButton != null)
            offButton.onClick.AddListener(() => MusicManager.Instance.StopMusic());
    }
}
