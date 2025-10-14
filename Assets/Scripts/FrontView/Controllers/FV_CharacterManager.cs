using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public GameObject[] lista;
    private Vector2 pos;
    private GameObject player;
    private int n = 0;
    private FVHook scriptSapo;

    // Start is called before the first frame update
    void Start()
    {
        player = lista[n];
        scriptSapo = lista[2].GetComponent<FVHook>();
    }

    // Update is called once per frame
    void Update()
    {
        pos = player.transform.position;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            n = 0;
            UpdatePlayer();
        } 
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            n = 1;
            UpdatePlayer();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            n = 2;
            scriptSapo.DesactiveHookAndPull();
            UpdatePlayer();
            
        }

        
    }

    void UpdatePlayer()
    {
        for (int i = 0; i < lista.Length; i++)
        {
            if (i == n)
            {
                player = lista[n];
                player.transform.position = pos;
                lista[i].SetActive(true);
            }
            else
            {
                lista[i].SetActive(false);
            }
        }
    }

}
