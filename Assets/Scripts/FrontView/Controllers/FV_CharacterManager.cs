using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{
    public GameObject[] lista;
    private Vector2 pos;
    private GameObject player;
    public int n = 0;
    private FVHook scriptSapo;
    private SaveData data;
    private GameObject StartPoint;
    private GameObject EndPoint;

    public LayerMask stopColliders;
    private Agarrar agarrarScriptDruida;
    private Agarrar agarrarScriptBear;

   

    
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
        agarrarScriptBear = lista[1].GetComponent<Agarrar>();
        agarrarScriptDruida = lista[0].GetComponent<Agarrar>();

        //Depende si el nivel esta completo, empezar en un punto u otro
        //string currentLevelName = SceneManager.GetActiveScene().name;
        //if(data.completedLevels.Contains(currentLevelName))
        //{
            //Si el nivel está completado, empezar desde el EndPoint
            //player.transform.position = EndPoint.transform.position;
        //}else
        //{
            //Si no, empezar desde el StartPoint
            player.transform.position = StartPoint.transform.position;
        //}


    }

    private void soltarCaja()
    {
        if(n == 0)
        {
            agarrarScriptDruida.agarrado = false;
            if(agarrarScriptDruida.cajaAgarrada != null)
            {
                agarrarScriptDruida.cajaAgarrada.parent = null;
                agarrarScriptDruida.cajaAgarrada = null;
            }
        }else if(n == 1)
        {
            agarrarScriptBear.agarrado = false;
            if(agarrarScriptBear.cajaAgarrada != null)
            {
                agarrarScriptBear.cajaAgarrada.parent = null;
                agarrarScriptBear.cajaAgarrada = null;
            }
        }
    }

    IEnumerator FixBug()
    {
        yield return new WaitForSeconds(0.01f);
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

        Vector2 origin = player.transform.position + Vector3.up * 0.8f;

        RaycastHit2D canTransformBearRight = Physics2D.Raycast(origin, Vector2.right, 1f, stopColliders);
        RaycastHit2D canTransformBearLeft = Physics2D.Raycast(origin, Vector2.left, 1f, stopColliders);

        RaycastHit2D canTransformBearRight2 = Physics2D.Raycast(player.transform.position, Vector2.right, 1f, stopColliders);
        RaycastHit2D canTransformBearLeft2 = Physics2D.Raycast(player.transform.position, Vector2.left, 1f, stopColliders);

        RaycastHit2D canTransformBearUp = Physics2D.Raycast(origin, Vector2.up, 1.3f, stopColliders);

        RaycastHit2D canTransform = Physics2D.Raycast(player.transform.position, Vector2.up, 1f, stopColliders);

        Debug.DrawRay(origin, Vector2.right * 1f, Color.red);
        Debug.DrawRay(origin, Vector2.left * 1f, Color.red);

        Debug.DrawRay(player.transform.position, Vector2.left * 1f, Color.red);
        Debug.DrawRay(player.transform.position, Vector2.right* 1f, Color.red);

        Debug.DrawRay(player.transform.position, Vector2.up * 1.3f, Color.red);

        if(canTransform.collider == null || (n != 2))
        {
            //Si no es ardilla o si es ardilla y puede transformarse
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                soltarCaja();
                //Druida
                n = 0;
                UpdatePlayer();
            } 
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {   
                //Si no hay obstaculos para transformarse en oso
                if(canTransformBearRight.collider == null && canTransformBearLeft.collider == null 
                    && canTransformBearUp.collider == null && canTransformBearRight2.collider == null 
                    && canTransformBearLeft2.collider == null)
                {
                    soltarCaja();
                    //Oso
                    n = 1;
                    UpdatePlayer();
                }
                

            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                //Ardilla

                //Comprobar si el personaje esta desbloqueado
                if(data.unlockedCharacters.Contains("Squirrel"))
                {
                    soltarCaja();
                    n = 2;
                    UpdatePlayer(); 
                }else
                {
                    Debug.Log("Personaje no desbloqueado");
                }
                
            }
        
            else if(Input.GetKeyDown(KeyCode.Alpha4))
            {
                soltarCaja();
                //Sapo
                n = 3;
                UpdatePlayer();
                //Desactivar el hookSapo NO TOCAR
                scriptSapo.DesactiveHookAndPull();
                
                
            }
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
