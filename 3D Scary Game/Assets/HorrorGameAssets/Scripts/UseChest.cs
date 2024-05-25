using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UseChest : MonoBehaviour
{
    private GameObject OB;
    public GameObject handUI;
    public GameObject objToActivate;

    private bool inReach;

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
        OB = this.gameObject;
        handUI.SetActive(false);
        objToActivate.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = true;
            handUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = false;
            handUI.SetActive(false);
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (inReach)
        {
            handUI.SetActive(false);
            objToActivate.SetActive(true);
            OB.GetComponent<Animator>().SetBool("open", true);
            OB.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
