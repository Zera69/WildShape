using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FVPlate : MonoBehaviour
{
    private bool activated = false;
    public GameObject linkedObject;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activated && collision.gameObject.CompareTag("box"))
        {
            activated = true;
            if(linkedObject.GetComponent<FVDoor>() != null)
            {
              FVDoor DoorScript = linkedObject.GetComponent<FVDoor>();
              DoorScript.Action();  
            }
        }
    }


}
