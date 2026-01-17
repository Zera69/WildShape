using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDCharacterManager : MonoBehaviour
{
    public GameObject[] lista;
    private Vector2 pos;
    private GameObject player;
    public int n = 0;

    public Transform movePoint;
    public LayerMask stopColliders;
    private TDCharacterMovement characterMovement;

    // Start is called before the first frame update
    void Start()
    {
        player = lista[n];
        characterMovement = FindObjectOfType<TDCharacterMovement>();
        
    }

    // Update is called once per frame
    void Update()
    {
        pos = player.transform.position;
        //si el personaje no se esta  moviendo, puede transformarse
        if (characterMovement.IsMoving == false)
        {
            
        
            if (n == 1)
            {
                //Si el oso se transforma, adaprtamos la posici n
                pos.x += 0.5f;
                pos.y -= 0.5f;
            }

            //Si la ardilla est  en un arbusto donde solo entra ella, no se puede transformar

            RaycastHit2D notBush = Physics2D.Raycast(player.transform.position, new Vector3(0f, 0f, 0f), 1f, stopColliders);
            if (n != 2 || (n == 2 && notBush.collider == null))
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    //Druida

                    n = 0;
                    UpdatePlayer();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    //Oso

                    RaycastHit2D free1 = Physics2D.Raycast(player.transform.position, new Vector3(0f, 1f, 0f), 1f, stopColliders);
                    RaycastHit2D free2 = Physics2D.Raycast(player.transform.position, new Vector3(-1f, 0f, 0f), 1f, stopColliders);
                    RaycastHit2D free3 = Physics2D.Raycast(player.transform.position, new Vector3(-1f, 1f, 0f), 1f, stopColliders);

                    Debug.DrawRay(player.transform.position, new Vector3(0f, 1f, 0f) * 1f, Color.red);
                    Debug.DrawRay(player.transform.position, new Vector3(-1f, 0f, 0f) * 1f, Color.red);
                    Debug.DrawRay(player.transform.position, new Vector3(-1f, 1f, 0f) * 1f, Color.red);

                    if (n != 1 && free1.collider == null && free2.collider == null && free3.collider == null)
                    {
                        pos.x -= 0.5f;
                        pos.y += 0.5f;

                        n = 1;
                        UpdatePlayer();
                    }

                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    //Ardilla

                    n = 2;
                    UpdatePlayer();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    //Sapo

                    n = 3;
                    UpdatePlayer();
                }
                
            }
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
                movePoint.position = pos;
                lista[i].SetActive(true);
            }
            else
            {
                lista[i].SetActive(false);
            }
        }
    }

}
