using UnityEngine;
using UnityEngine.InputSystem;

public class ActivateUI : MonoBehaviour
{
    public GameObject handUI;
    public GameObject objToActivate;
    private GameObject OB;

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
            objToActivate.SetActive(true);
        }
    }

}


