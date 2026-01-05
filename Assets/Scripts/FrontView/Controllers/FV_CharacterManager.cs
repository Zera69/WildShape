using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{
    public GameObject[] lista;
    private Vector2 pos;
    private GameObject player;
    private int n = 0;
    private FVHook scriptSapo;
    private SaveData data;
    private GameObject StartPoint;
    private GameObject EndPoint;

   

    
    // Start is called before the first frame update
    void Start()
    {
        StartPoint = GameObject.FindGameObjectWithTag("StartPoint");
        EndPoint = GameObject.FindGameObjectWithTag("EndPoint");
        player = lista[n];
        scriptSapo = lista[2].GetComponent<FVHook>();
        data = SaveManager.instance.GetData();
        string currentLevelName = SceneManager.GetActiveScene().name;
        if(data.completedLevels.Contains(currentLevelName))
        {
            //Si el nivel está completado, empezar desde el EndPoint
            player.transform.position = EndPoint.transform.position;
        }else
        {
            //Si no, empezar desde el StartPoint
            player.transform.position = StartPoint.transform.position;
        }


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
            //Comprobar si el personaje esta desbloqueado
            if(data.unlockedCharacters.Contains("Squirrel"))
            {
               n = 1;
                UpdatePlayer(); 
            }else
            {
                Debug.Log("Personaje no desbloqueado");
            }
            
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
