using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseChest : MonoBehaviour
{
    private GameObject OB;
    public GameObject handUI;
    public GameObject objToActivate;


    private bool inReach;


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

    void Update()
    {


        if (inReach && Input.GetButtonDown("Interact"))
        {
            handUI.SetActive(false);
            objToActivate.SetActive(true);
            OB.GetComponent<Animator>().SetBool("open", true);
            OB.GetComponent<BoxCollider>().enabled = false;
        }
    }

}
