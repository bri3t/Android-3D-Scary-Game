using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 5.0f;  // Velocidad de movimiento del personaje
    public float rotationSpeed = 700.0f;  // Velocidad de rotación de la cámara

    private Vector2 moveInput;      // Vector para almacenar la entrada del joystick
    private CharacterController characterController;
    private Camera mainCamera;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }

    // Se llama cuando se procesa la entrada de movimiento
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();  // Lee el vector de movimiento del joystick
    }

    // Se llama cuando se procesa la entrada de toque en la pantalla
    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.performed && context.control.device is Pointer pointer)
        {
            Vector2 touchPosition = pointer.position.ReadValue();
            if (touchPosition.x > Screen.width / 2)
            {
                Debug.Log("toca pantalla");
                RotateCamera(context.ReadValue<Vector2>());
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);  // Calcula el vector de movimiento
        characterController.Move(moveSpeed * Time.deltaTime * movement);  // Aplica el movimiento al CharacterController
    }

    private void RotateCamera(Vector2 lookInput)
    {
        float rotationY = lookInput.x * rotationSpeed * Time.deltaTime;
        mainCamera.transform.Rotate(0, rotationY, 0, Space.World);
    }
}
