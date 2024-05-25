using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MveAux : MonoBehaviour

{

    // Camera:
    public Camera playerCam;

    public float moveSpeed = 5.0f;  // Velocidad de movimiento del personaje
    public float rotationSpeed = 100.0f;  // Velocidad de rotación de la cámara

    private Vector2 moveInput;      // Vector para almacenar la entrada del joystick
    private Vector2 lookInput;      // Vector para almacenar la entrada de la rotación de la cámara
    private Camera mainCamera;

    private PlayerInputActions inputActions;

    private void Awake()
    {
        mainCamera = Camera.main;
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Jugador.Enable();
        inputActions.Jugador.Look.performed += OnLook;
        inputActions.Jugador.Look.canceled += OnLookCanceled;
    }

    private void OnDisable()
    {
        inputActions.Jugador.Look.performed -= OnLook;
        inputActions.Jugador.Look.canceled -= OnLookCanceled;
        inputActions.Jugador.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 touchPosition = Pointer.current.position.ReadValue();
        if (touchPosition.x > Screen.width * 0.4f)  // Solo activa OnLook si el toque está en el 70% derecho de la pantalla
        {
            lookInput = context.ReadValue<Vector2>();
        }
    }

    public void OnLookCanceled(InputAction.CallbackContext context)
    {
        lookInput = Vector2.zero;
    }

    private void Update()
    {
        // Movimiento del personaje
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);
        movement = mainCamera.transform.TransformDirection(movement);
        movement.y = 0f; // Asegúrate de que el movimiento en el eje Y sea cero
        movement.Normalize();
        transform.Translate(moveSpeed * movement * Time.deltaTime, Space.World);

        // Rotación de la cámara
        if (lookInput != Vector2.zero)
        {
            RotateCamera(lookInput);
        }
    }

    private void RotateCamera(Vector2 lookInput)
    {
        float rotationX = -lookInput.y * rotationSpeed * Time.deltaTime;  // Rotación vertical (invertida)
        float rotationY = lookInput.x * rotationSpeed * Time.deltaTime;   // Rotación horizontal

        // Asegura que la cámara no se incline
        mainCamera.transform.Rotate(new Vector3(rotationX, rotationY, 0), Space.Self);

        // Clamping the rotation to avoid flipping the camera
        Vector3 currentEulerAngles = mainCamera.transform.localEulerAngles;
        currentEulerAngles.z = 0; // No inclinación
        mainCamera.transform.localEulerAngles = currentEulerAngles;
    }
}