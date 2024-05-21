using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanturnPickUp : MonoBehaviour
{
    private GameObject OB;
    public GameObject handUI;
    public GameObject lanturn;


    private bool inReach;


    void Start()
    {
        OB = this.gameObject;

        handUI.SetActive(false);

        lanturn.SetActive(false);

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
            lanturn.SetActive(true);
            StartCoroutine(end());
        }
    }

    IEnumerator end()
    {
        yield return new WaitForSeconds(.01f);
        Destroy(OB);
    }
}
