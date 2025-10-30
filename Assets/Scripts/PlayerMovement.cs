using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
   public InputSystem_Actions actions;
    [SerializeField] private float moveSpeed = 5f;

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
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);

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

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        // Prevent spamming the animation
        if (!isInteracting)
            StartCoroutine(PlayInteractAnimation());
    }

    private IEnumerator PlayInteractAnimation()
    {
        isInteracting = true;

        // Stop movement while interacting (optional)
        moveInput = Vector2.zero;
        rb.linearVelocity = Vector2.zero;

        animator.SetTrigger("Interact");

        // Adjust the delay to your animation’s actual length
        yield return new WaitForSeconds(0.7f);

        isInteracting = false;
    }

    public void Movement(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void Update()
    {
        if (rb == null || isInteracting)
            return;

        // Move player using Rigidbody2D
        rb.linearVelocity = moveInput * moveSpeed;

        // Animate movement
        bool isMoving = moveInput.magnitude > 0.01f;
        animator.SetBool("isMoving", isMoving);

        // Flip sprite horizontally
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
            actions.UI.Disable(); // if you use UI map
        }
    }
}