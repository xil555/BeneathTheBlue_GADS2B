using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    public static PlayerInput PlayerInput;

    private InputAction _moveAction;
    private InputAction _interactAction;

    public static Vector2 MoveInput;
    public static bool WasInteractPressed;

    [Header("Attack Settings")]
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;
    private float lastAttackTime = 0f;

    private void OnEnable()
    {
        if (PlayerInput == null)
            PlayerInput = GetComponent<PlayerInput>();
        PlayerInput.actions.Enable();
    }

    private void Start()
    {
        PlayerInput = GetComponent<PlayerInput>();
        _moveAction = PlayerInput.actions.FindAction("Move");
        _interactAction = PlayerInput.actions.FindAction("Interact");
    }

    private void Update()
    {
        if (_moveAction != null)
            MoveInput = _moveAction.ReadValue<Vector2>();
        else
            MoveInput = Vector2.zero;

        if (_interactAction != null)
            WasInteractPressed = _interactAction.WasPressedThisFrame();
        else
            WasInteractPressed = false;

        // Attack with cooldown
        if (WasInteractPressed && Time.time - lastAttackTime >= attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    private void Attack()
    {
        // hit objects tagged "Bottle"
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Bottle"))
            {
                DestructibleObject obj = hit.GetComponent<DestructibleObject>();
                if (obj != null)
                    obj.Hit();
            }
        }
    }

    private void OnDisable()
    {
        if (PlayerInput != null)
            PlayerInput.actions.Disable(); // disable action maps to prevent leaks
    }

    // Remove OnDestroy() entirely or leave empty
    private void OnDestroy()
    {
        // nothing needed here
    }
}

