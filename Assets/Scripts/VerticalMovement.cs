using UnityEngine;

public class VerticalMovement : MonoBehaviour
{
   [SerializeField] public float speed = 2f;           // How fast it moves
    [SerializeField] public float height = 1f;          // How far up and down it moves
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Make it move smoothly up and down
        transform.position = startPos + Vector3.up * Mathf.Sin(Time.time * speed) * height;
    }
}
