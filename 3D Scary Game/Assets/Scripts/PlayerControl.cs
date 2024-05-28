using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    // Camera:
    public Camera playerCam;  // Referencia a la cámara del jugador

    // Movement Settings:
    public float walkSpeed = 5.0f;  // Velocidad al caminar
    public float runSpeed = 5f;  // Velocidad al correr
    public float jumpPower = 0f;  // Potencia de salto
    public float gravity = 10f;  // Gravedad aplicada al jugador

    // Camera Settings:
    public float lookSpeed = 2f;  // Velocidad de rotación de la cámara
    public float lookXLimit = 75f;  // Límite de rotación en el eje X para evitar rotaciones extremas
    public float cameraRotationSmooth = 5f;  // Suavidad de la rotación de la cámara

    // Ground Sounds:
    public AudioClip[] woodFootstepSounds;  // Sonidos de pasos en madera
    public AudioClip[] tileFootstepSounds;  // Sonidos de pasos en baldosas
    public AudioClip[] carpetFootstepSounds;  // Sonidos de pasos en alfombra
    public Transform footstepAudioPosition;  // Posición del audio de pasos
    public AudioSource audioSource;  // Componente de audio para reproducir los sonidos de pasos

    private bool isWalking = false;  // Indica si el jugador está caminando
    private bool isFootstepCoroutineRunning = false;  // Indica si la corutina de sonidos de pasos está en ejecución
    private AudioClip[] currentFootstepSounds;  // Sonidos de pasos actuales dependiendo de la superficie

    private Vector3 moveDirection = Vector3.zero;  // Dirección de movimiento del jugador
    private float rotationX = 0;  // Rotación en el eje X (vertical)
    private float rotationY = 0;  // Rotación en el eje Y (horizontal)

    private bool canMove = true;  // Indica si el jugador puede moverse

    private Vector2 moveInput;  // Entrada de movimiento del jugador
    private Vector2 lookInput;  // Entrada de rotación de la cámara

    private PlayerInputActions inputActions;  // Acciones de entrada del jugador

    private void Awake()
    {
        // Inicializar las acciones de entrada del jugador
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        // Habilitar las acciones de entrada y suscribirse a los eventos de entrada
        inputActions.Jugador.Enable();
        inputActions.Jugador.Look.performed += OnLook;
        inputActions.Jugador.Look.canceled += OnLookCanceled;
    }

    private void OnDisable()
    {
        // Desuscribirse de los eventos de entrada y deshabilitar las acciones de entrada
        inputActions.Jugador.Look.performed -= OnLook;
        inputActions.Jugador.Look.canceled -= OnLookCanceled;
        inputActions.Jugador.Disable();
    }

    private void Start()
    {
        // Inicializar los sonidos de pasos actuales a los sonidos de madera
        currentFootstepSounds = woodFootstepSounds;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // Leer la entrada de movimiento del jugador
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnMoveCanceled(InputAction.CallbackContext context)
    {
        // Reiniciar la entrada de movimiento del jugador cuando se cancela
        moveInput = Vector2.zero;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        // Leer la posición del toque
        Vector2 touchPosition = Pointer.current.position.ReadValue();
        // Solo activar la rotación de la cámara si el toque está en el 60% derecho de la pantalla
        if (touchPosition.x > Screen.width * 0.4f)
        {
            lookInput = context.ReadValue<Vector2>();
        }
    }

    public void OnLookCanceled(InputAction.CallbackContext context)
    {
        // Reiniciar la entrada de rotación de la cámara cuando se cancela
        lookInput = Vector2.zero;
    }

    private void Update()
    {
        // Movimiento del personaje
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);
        movement = playerCam.transform.TransformDirection(movement);
        movement.y = 0f; // Asegurarse de que el movimiento en el eje Y sea cero
        movement.Normalize();
        footstepAudioPosition.Translate(walkSpeed * movement * Time.deltaTime, Space.World);

        // Rotación de la cámara
        if (canMove)
        {
            RotateCamera(lookInput);
        }

        // Reproducción de sonidos de pasos
        if ((movement.x != 0f || movement.z != 0f) && !isWalking && !isFootstepCoroutineRunning)
        {
            isWalking = true;
            StartCoroutine(PlayFootstepSounds(2f / walkSpeed));
        }
        else if (movement.x == 0f && movement.z == 0f)
        {
            isWalking = false;
        }
    }

    private void RotateCamera(Vector2 lookInput)
    {
        // Actualizar la rotación en el eje X (vertical) basada en la entrada del jugador
        rotationX -= lookInput.y * lookSpeed;
        // Limitar la rotación en el eje X para evitar rotaciones extremas
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

        // Actualizar la rotación en el eje Y (horizontal) basada en la entrada del jugador
        rotationY += lookInput.x * lookSpeed;

        // Crear las rotaciones objetivo en los ejes X e Y usando Quaternion
        Quaternion targetRotationX = Quaternion.Euler(rotationX, 0, 0);
        Quaternion targetRotationY = Quaternion.Euler(0, rotationY, 0);

        // Interpolar suavemente la rotación de la cámara hacia la rotación objetivo en X
        playerCam.transform.localRotation = Quaternion.Slerp(playerCam.transform.localRotation, targetRotationX, Time.deltaTime * cameraRotationSmooth);
        // Interpolar suavemente la rotación del objeto del jugador hacia la rotación objetivo en Y
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationY, Time.deltaTime * cameraRotationSmooth);
    }

    // Reproducción de sonidos de pasos con un retraso basado en la velocidad de movimiento
    private IEnumerator PlayFootstepSounds(float footstepDelay)
    {
        isFootstepCoroutineRunning = true;

        while (isWalking)
        {
            if (currentFootstepSounds.Length > 0)
            {
                int randomIndex = Random.Range(0, currentFootstepSounds.Length);
                audioSource.transform.position = footstepAudioPosition.position;
                audioSource.clip = currentFootstepSounds[randomIndex];
                audioSource.Play();
                yield return new WaitForSeconds(footstepDelay);
            }
            else
            {
                yield break;
            }
        }

        isFootstepCoroutineRunning = false;
    }

    // Detección de la superficie del suelo y configuración de la matriz de sonidos de pasos actual
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wood"))
        {
            currentFootstepSounds = woodFootstepSounds;
        }
    }
}
