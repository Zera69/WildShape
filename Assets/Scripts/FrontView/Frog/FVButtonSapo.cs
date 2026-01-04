using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FVButtonSapo : MonoBehaviour
{

    private bool activated = false;
    public GameObject linkedObject;

    public void Activate()
    {
        if (!activated)
        {
            activated = true;
            if(linkedObject.GetComponent<FVDoor>() != null)
            {
              FVDoor DoorScript = linkedObject.GetComponent<FVDoor>();
              //DoorScript.Action();  
            }
            if(linkedObject.GetComponent<SceneLoadManager>() != null)
            {
              SceneLoadManager NextScene = linkedObject.GetComponent<SceneLoadManager>();
              NextScene.Active();  
            }
        }

    }

}
