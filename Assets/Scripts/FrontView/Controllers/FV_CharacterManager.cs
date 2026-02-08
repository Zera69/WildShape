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
    private  Vector2 origin;

    private  RaycastHit2D canTransformBearRight;
    private  RaycastHit2D canTransformBearLeft ;

    private  RaycastHit2D canTransformBearRight2 ;
    private  RaycastHit2D canTransformBearLeft2;

    private  RaycastHit2D canTransformBearUp;

    private RaycastHit2D canTransform;
    private TransformWheel transformWheel;

   

    
    // Start is called before the first frame update
    void Start()
    {
        TransformWheel transformWheel = FindObjectOfType<TransformWheel>();
        
        //puntos de spawn
        StartPoint = GameObject.FindGameObjectWithTag("StartPoint");
        EndPoint = GameObject.FindGameObjectWithTag("EndPoint");

        //empezar con druida 
        player = lista[0];

        //Coger la data de guardado
        data = SaveManager.instance.GetData();

        //Configurar el personaje actual segun el guardado
        n = data.CurrentCharacterIndex;

        scriptSapo = lista[2].GetComponent<FVHook>();
        StartCoroutine(FixBug());
        agarrarScriptBear = lista[1].GetComponent<Agarrar>();
        agarrarScriptDruida = lista[0].GetComponent<Agarrar>();

        player.transform.position = StartPoint.transform.position;
        


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
        n = 2;
        UpdatePlayer();
        scriptSapo.DesactiveHookAndPull();
        n = data.CurrentCharacterIndex;
        UpdatePlayer();
    }
        
    

    // Update is called once per frame
    void Update()
    {
        pos = player.transform.position;

        origin = player.transform.position + Vector3.up * 0.8f;

        canTransformBearRight = Physics2D.Raycast(origin, Vector2.right, 1f, stopColliders);
        canTransformBearLeft = Physics2D.Raycast(origin, Vector2.left, 1f, stopColliders);

        canTransformBearRight2 = Physics2D.Raycast(player.transform.position, Vector2.right, 1f, stopColliders);
        canTransformBearLeft2 = Physics2D.Raycast(player.transform.position, Vector2.left, 1f, stopColliders);

        canTransformBearUp = Physics2D.Raycast(origin, Vector2.up, 1.3f, stopColliders);

        canTransform = Physics2D.Raycast(player.transform.position, Vector2.up, 1f, stopColliders);

        Debug.DrawRay(origin, Vector2.right * 1f, Color.red);
        Debug.DrawRay(origin, Vector2.left * 1f, Color.red);

        Debug.DrawRay(player.transform.position, Vector2.left * 1f, Color.red);
        Debug.DrawRay(player.transform.position, Vector2.right* 1f, Color.red);

        Debug.DrawRay(player.transform.position, Vector2.up * 1.3f, Color.red);

        if(canTransform.collider == null || (n != 2))
        {
            if(Input.GetKeyUp(KeyCode.Tab))
            {
                if(transformWheel == null)
                {
                    transformWheel = FindObjectOfType<TransformWheel>();
                }

                if(transformWheel != null)
                {
                    if(transformWheel.currentDirection == TransformWheel.WheelDirection.Up)
                    {
                        TransformDruida();
                    }else if(transformWheel.currentDirection == TransformWheel.WheelDirection.Down)
                    {
                        TransformBear();
                    }else if(transformWheel.currentDirection == TransformWheel.WheelDirection.Left)
                    {
                        TransformSquirrel();
                    }else if(transformWheel.currentDirection == TransformWheel.WheelDirection.Right)
                    {
                        TransformToad();
                    }
                }
            }

            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                TransformDruida();
            }else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                TransformBear();
            }else if(Input.GetKeyDown(KeyCode.Alpha3))
            {
                TransformToad();
            }else if(Input.GetKeyDown(KeyCode.Alpha4))
            {
                TransformSquirrel();
            }
            
        }

        
    }

    public void TransformDruida()
    {
        if (data.unlockedCharacters.Contains("Druid"))
        {
            soltarCaja();
            //Druida
            n = 0;
            UpdatePlayer();
        }
        else
        {
            Debug.Log("Personaje no desbloqueado");
        }
        
    }

    public void TransformBear()
    {
        if (data.unlockedCharacters.Contains("Bear"))
        {
            if (canTransformBearRight.collider == null && canTransformBearLeft.collider == null
            && canTransformBearUp.collider == null && canTransformBearRight2.collider == null
            && canTransformBearLeft2.collider == null)
            {
                soltarCaja();
                //Oso
                n = 1;
                UpdatePlayer();
            }
        }
        else
        {
            Debug.Log("Personaje no desbloqueado");
        }
        //Si no hay obstaculos para transformarse en oso
        
    }

    public void TransformToad()
    {
        if (data.unlockedCharacters.Contains("Toad"))
        {
            soltarCaja();
            //Sapo
            n = 2;
            UpdatePlayer();
            //Desactivar el hookSapo NO TOCAR
            scriptSapo.DesactiveHookAndPull();
        }
        else
        {
            Debug.Log("Personaje no desbloqueado");
        }
        
    }

    public void TransformSquirrel()
    {
        //Comprobar si el personaje esta desbloqueado 
        if (data.unlockedCharacters.Contains("Squirrel"))
        {
            soltarCaja();
            n = 3;
            UpdatePlayer(); 
        }else
        {
            Debug.Log("Personaje no desbloqueado");
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
        AudioManager.Instance.PlaySFX("transform");
    }

}
