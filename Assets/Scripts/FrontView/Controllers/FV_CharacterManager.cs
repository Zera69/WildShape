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
        
        //puntos de spawn
        StartPoint = GameObject.FindGameObjectWithTag("StartPoint");
        EndPoint = GameObject.FindGameObjectWithTag("EndPoint");

        //empezar con druida 
        player = lista[0];

        //Coger la data de guardado
        data = SaveManager.instance.GetData();

        //Configurar el personaje actual segun el guardado
        n = data.CurrentCharacterIndex;

        scriptSapo = lista[3].GetComponent<FVHook>();
        StartCoroutine(FixBug());

        //Depende si el nivel esta completo, empezar en un punto u otro
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

    IEnumerator FixBug()
    {
        yield return new WaitForSeconds(0.03f);
        n = 3;
        UpdatePlayer();
        scriptSapo.DesactiveHookAndPull();
        n = data.CurrentCharacterIndex;
        UpdatePlayer();
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
            UpdatePlayer();
            //Desactivar el hookSapo NO TOCAR
            scriptSapo.DesactiveHookAndPull();
            
            
        }

        
    }

    public int GetCurrentCharacterIndex()
    {
        return n;
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
