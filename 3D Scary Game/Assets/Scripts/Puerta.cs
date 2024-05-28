using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace PuertaScript
{
    [RequireComponent(typeof(AudioSource))]
    public class Puerta : MonoBehaviour
    {
        public bool open;
        public float smooth = 1.0f;
        float DoorOpenAngle = -90.0f;
        float DoorCloseAngle = 0.0f;
        public AudioSource asource;
        public AudioClip openDoor, closeDoor;

        public GameObject handUI;

        private bool inReach;
        private bool locked;
        private PlayerInputActions inputActions;

        private void Awake()
        {
            inputActions = new PlayerInputActions();
            inputActions.Jugador.Interact.performed += OnInteract;
        }

        private void OnEnable()
        {
            inputActions.Jugador.Enable();
        }

        private void OnDisable()
        {
            inputActions.Jugador.Disable();
        }

        void Start()
        {
            asource = GetComponent<AudioSource>();
            handUI.SetActive(false);
            locked = false;
        }

        void Update()
        {
            if (open)
            {
                var target = Quaternion.Euler(0, DoorOpenAngle, 0);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * 5 * smooth);
            }
            else
            {
                var target1 = Quaternion.Euler(0, DoorCloseAngle, 0);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, target1, Time.deltaTime * 5 * smooth);
            }
        }


        public void Cerrar() {

            locked = true; // Bloquea la puerta cuando el jugador entra en el trigger "cerrarpuerta"
            handUI.SetActive(false); // Oculta la UI si la puerta está bloqueada
            if (open)
            {
                open = false;
                asource.clip = closeDoor;
                asource.Play();
            }
        }

      
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Reach" && !locked)
            {
                inReach = true;
                handUI.SetActive(true);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Reach" && !locked)
            {
                inReach = false;
                handUI.SetActive(false);
            }
        }



        private void OnInteract(InputAction.CallbackContext context)
        {
            if (inReach && !locked) // Solo permite la interacción si la puerta no está bloqueada
            {
                open = !open;
                asource.clip = open ? openDoor : closeDoor;
                asource.Play();
                handUI.SetActive(false);
            }
        }
    }
}