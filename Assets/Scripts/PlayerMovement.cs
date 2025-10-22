using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputSystem_Actions actions;
    [SerializeField] private float moveSpeed = 400f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isInteracting = false;

    public static PlayerMovement Instance;

    private void Awake()
    {
        // Singleton pattern — ensure only one persistent player
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        transform.SetParent(null);          // detach from any parent
        DontDestroyOnLoad(gameObject);      // persist across scenes

        // Initialize input system
        if (actions == null)
            actions = new InputSystem_Actions();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) Debug.LogError("[PlayerMovement] Rigidbody2D missing!");

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Enable and bind input actions
        actions.Player.Enable();
        actions.Player.Move.performed += Movement;
        actions.Player.Move.canceled += Movement;
        actions.Player.Interact.performed += OnInteract;
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!isInteracting)
            StartCoroutine(PlayInteractAnimation());
    }

    private IEnumerator PlayInteractAnimation()
    {
        isInteracting = true;
        animator.SetTrigger("Interact");
        yield return new WaitForSeconds(0.5f); // adjust to your animation length
        isInteracting = false;
    }

    private void Movement(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void Update()
    {
        // Move player using Rigidbody2D
        if (rb != null)
            rb.linearVelocity = moveInput * moveSpeed * Time.deltaTime;

        // Animate movement
        bool isMoving = moveInput.magnitude > 0.01f;
        animator.SetBool("isMoving", isMoving);

        // Flip sprite horizontally based on movement
        if (moveInput.x > 0.01f)
            spriteRenderer.flipX = false;
        else if (moveInput.x < -0.01f)
            spriteRenderer.flipX = true;
    }
    private void OnDestroy()
    {
        if (actions != null)
        {
            actions.Player.Disable();
            actions.UI.Disable(); // if you use a UI action map
        }
    }
}