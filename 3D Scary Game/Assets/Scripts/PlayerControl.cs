using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 5.0f;  // Velocidad de movimiento del personaje
    public float rotationSpeed = 100.0f;  // Velocidad de rotaci�n de la c�mara

    private Vector2 moveInput;      // Vector para almacenar la entrada del joystick
    private Vector2 lookInput;      // Vector para almacenar la entrada de la rotaci�n de la c�mara
    private CharacterController characterController;
    private Camera mainCamera;

    private PlayerInputActions inputActions;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;

        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Jugador.Enable();
        inputActions.Jugador.Move.performed += OnMove;
        inputActions.Jugador.Look.performed += OnLook;
        inputActions.Jugador.Look.canceled += OnLookCanceled;
    }

    private void OnDisable()
    {
        inputActions.Jugador.Move.performed -= OnMove;
        inputActions.Jugador.Look.performed -= OnLook;
        inputActions.Jugador.Look.canceled -= OnLookCanceled;
        inputActions.Jugador.Disable();
    }

    // Se llama cuando se procesa la entrada de movimiento
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();  // Lee el vector de movimiento del joystick
    }

    // Se llama cuando se procesa la entrada de toque en la pantalla
    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 touchPosition = Pointer.current.position.ReadValue();
        if (touchPosition.x > Screen.width / 2)
        {
            lookInput = context.ReadValue<Vector2>();  // Lee el vector de rotaci�n de la c�mara
        }
    }

    // Se llama cuando se deja de procesar la entrada de toque en la pantalla
    public void OnLookCanceled(InputAction.CallbackContext context)
    {
        lookInput = Vector2.zero;
    }

    private void Update()
    {
        // Movimiento del personaje
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);
        movement = mainCamera.transform.TransformDirection(movement);
        movement.y = 0f;
        characterController.Move(moveSpeed * Time.deltaTime * movement);

        // Rotaci�n de la c�mara
        if (lookInput != Vector2.zero)
        {
            RotateCamera(lookInput);
        }
    }

    private void RotateCamera(Vector2 lookInput)
    {
        float rotationX = -lookInput.y * rotationSpeed * Time.deltaTime;  // Rotaci�n vertical (invertida)
        float rotationY = lookInput.x * rotationSpeed * Time.deltaTime;   // Rotaci�n horizontal

        // Rotar la c�mara en ambas direcciones
        mainCamera.transform.Rotate(rotationX, rotationY, 0);

        // Asegura que la c�mara no se incline
        Vector3 currentEulerAngles = mainCamera.transform.eulerAngles;
        mainCamera.transform.eulerAngles = new Vector3(currentEulerAngles.x, currentEulerAngles.y, 0);
    }
}
