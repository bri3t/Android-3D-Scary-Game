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
   


    void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Player") // Solo muestra la UI si la puerta no está bloqueada
        {
            Debug.Log("Choca");
            puerta.Cerrar();

        }
    }

}
