using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
        //El chocarse con el player, desactivamos el collider para que no quite mas vida
        if (other.gameObject.tag == "Player")
        {
            gameObject.active = false;
        }
        //El chocarse con el enemigo, desactivamos tambien
        if (other.gameObject.tag == "enemy")
        {
            gameObject.active = false;
        }
        
    }
}
