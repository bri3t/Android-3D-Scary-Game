using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    // Cantidad de balanceo que se aplicará a la pistola
    public float swayAmount = 0.5f;

    // Factor de suavizado para el movimiento del balanceo
    public float smoothFactor = 2f;

    // Rotación inicial de la pistola
    private Quaternion initialRotation;

    // Referencia a la cámara del jugador
    private Transform playerCamera;

    // Método Start se llama al iniciar el script
    void Start()
    {
        // Obtener la referencia a la cámara principal del jugador
        playerCamera = Camera.main.transform;

        // Guardar la rotación inicial del objeto
        initialRotation = transform.localRotation;
    }

    // Método Update se llama una vez por frame
    void Update()
    {
        // Obtener el movimiento del ratón en el eje X e invertirlo para el balanceo
        float inputX = -Input.GetAxis("Mouse X") * swayAmount;

        // Obtener el movimiento del ratón en el eje Y e invertirlo para el balanceo
        float inputY = -Input.GetAxis("Mouse Y") * swayAmount;

        // Calcular la rotación objetivo aplicando el movimiento del ratón a la rotación inicial
        Quaternion targetRotation = Quaternion.Euler(inputY, inputX, 0f) * initialRotation;

        // Interpolar suavemente entre la rotación actual y la rotación objetivo
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * smoothFactor);
    }
}
