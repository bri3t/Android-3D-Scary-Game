using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;  // Velocidad de movimiento del personaje
    private Vector2 moveInput;      // Vector para almacenar la entrada del joystick


    // Se llama cuando se procesa la entrada de movimiento
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();  // Lee el vector de movimiento del joystick
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);  // Calcula el vector de movimiento
        movement.Normalize();
        transform.Translate(moveSpeed * movement * Time.deltaTime);  // Aplica el movimiento al Rigidbody
    }
}
