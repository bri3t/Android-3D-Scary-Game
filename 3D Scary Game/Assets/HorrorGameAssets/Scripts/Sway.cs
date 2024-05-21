using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    public float swayAmount = 0.5f;      // The amount of sway to apply to the pistol
    public float smoothFactor = 2f;      // The smooth factor for the sway movement

    private Quaternion initialRotation;  // The initial rotation of the pistol
    private Transform playerCamera;      // Reference to the player's camera

    void Start()
    {
        playerCamera = Camera.main.transform;
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        float inputX = -Input.GetAxis("Mouse X") * swayAmount;
        float inputY = -Input.GetAxis("Mouse Y") * swayAmount;

        Quaternion targetRotation = Quaternion.Euler(inputY, inputX, 0f) * initialRotation;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * smoothFactor);
    }
}

