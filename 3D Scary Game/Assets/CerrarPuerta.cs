using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using PuertaScript;

public class CerrarPuerta : MonoBehaviour
{
    public Puerta puerta;
    public GameObject objToActivate;



    private void Start()
    {
        objToActivate.SetActive(false);

    }

    void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Player") // Solo muestra la UI si la puerta no está bloqueada
        {
            objToActivate.SetActive(true);
            puerta.Cerrar();
        }
    }

}
