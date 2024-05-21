using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickUp : MonoBehaviour
{
    public GameObject handUI;
    public GameObject objToActivate;

    private GameObject ob;


    private bool inReach;


    void Start()
    {
        handUI.SetActive(false);

        objToActivate.SetActive(false);

        ob = this.gameObject;

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

    void Update()
    {


        if (inReach && Input.GetButtonDown("Interact"))
        {
            handUI.SetActive(false);
            objToActivate.SetActive(true);
            ob.GetComponent<MeshRenderer>().enabled = false;
        }
    }

}
