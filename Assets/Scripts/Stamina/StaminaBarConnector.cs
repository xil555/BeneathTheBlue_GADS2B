using UnityEngine;
using UnityEngine.UI;
public class StaminaBarConnector : MonoBehaviour
{
    [Header("Assign UI Stamina Bar for this scene")]
    public Image staminaBar;  // <-- this must be 'public' or [SerializeField]

    void Start()
    {
        if (staminaSystem.Instance != null && staminaBar != null)
        {
            staminaSystem.Instance.staminaBar = staminaBar;
        }
    }
}
