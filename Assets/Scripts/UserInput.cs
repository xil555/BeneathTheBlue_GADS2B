using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static PlayerInput PlayerInput;
      private Vector2 moveInput;
    public static Vector2 MoveInput;

    public static bool WasInteractPressed;

    private InputAction _moveAction;
    private InputAction _interactAction;

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        // Ensure PlayerInput and actions are ready here
        if (PlayerInput != null)
        {
            _moveAction = PlayerInput.actions["Move"];
            _interactAction = PlayerInput.actions["Interact"];
        }
        else
        {
            Debug.LogError("PlayerInput component not found!");
        }
    }

    // Update is called once per frame
    /*void Update()
    {
        MoveInput = _moveAction.ReadValue<Vector2>();
        WasInteractPressed = _interactAction.WasPressedThisFrame();
    }
    */
    public void Interact(InputAction.CallbackContext ctx)
    {
        // Don't read a Vector2 here — just check if the button was pressed
        if (ctx.performed)
        {
            WasInteractPressed = true;
            Debug.Log("Interact pressed!");
        }
    }

    private void Update()
    {

        if (_moveAction != null)
            MoveInput = _moveAction.ReadValue<Vector2>();
        else
            MoveInput = Vector2.zero;

        if (WasInteractPressed)
            WasInteractPressed = false;
    }
}


