using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnd : MonoBehaviour
{
    public MenuManager menuManager;
    private bool dentro = false;

    // Update is called once per frame
    void Update()
    {
        triggerEnd();
    }

    public void triggerEnd()
    {
        if(dentro && Input.GetKeyDown(KeyCode.E))
        {
            menuManager.triggerEnd();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            dentro = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            dentro = false;
        }
    }

}
