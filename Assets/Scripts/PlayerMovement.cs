using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public InputSystem_Actions actions;
    [SerializeField] float moveSpeed = 400f;
     Rigidbody2D rb;
    private Vector2 moveInput;
    public static PlayerMovement Instance;
    float move;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isInteracting = false;


    void Awake()
    {
        // Singleton fix — only one Input System instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

      
        actions = new InputSystem_Actions();
    }
        void Start()
    {
       rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Bind inputs
        actions.Player.Enable();
        actions.Player.Move.performed += Movement;
        actions.Player.Move.canceled += Movement;

        // Bind Interact (E or Left Click)
        actions.Player.Interact.performed += OnInteract;
    }
    void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!isInteracting)
            StartCoroutine(PlayInteractAnimation());
    }
    IEnumerator PlayInteractAnimation()
    {
        isInteracting = true;
        animator.SetTrigger("Interact");
        yield return new WaitForSeconds(0.5f); // adjust based on animation length
        isInteracting = false;
    }

    public void Movement(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }


    // Update is called once per frame
    void Update()

    {
        rb.linearVelocity = moveInput * moveSpeed;

        bool isMoving = moveInput.magnitude > 0.01f;
        animator.SetBool("isMoving", isMoving);

        // 🟢 Flip and face direction
        if (moveInput.x > 0.01f)
            spriteRenderer.flipX = false;
        else if (moveInput.x < -0.01f)
            spriteRenderer.flipX = true;

        
    }

}



  

