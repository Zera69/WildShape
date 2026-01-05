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
    private SaveData data;

    // Start is called before the first frame update
    void Start()
    {
        player = lista[n];
        scriptSapo = lista[2].GetComponent<FVHook>();
        data = SaveManager.instance.GetData();

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
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //Comprobar si el personaje esta desbloqueado
            if(data.unlockedCharacters.Contains("Squirrel"))
            {
               n = 2;
                UpdatePlayer(); 
            }else
            {
                Debug.Log("Personaje no desbloqueado");
            }
            
        }
        else if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            n = 3;
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
