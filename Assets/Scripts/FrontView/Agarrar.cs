using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agarrar : MonoBehaviour
{
    public bool agarrado = false;
    public LayerMask cajaLayer;
    public  LayerMask paredLayer;
    public double distanciaAgarrar = 1;
    private double distanciaPared;
    private FVDruida druidaMovement;
    private BearMovement bearMovement;
    private CharacterManager characterManager;
    private Vector2 lookDirection;
    public bool paredDelante = false;
    public Transform cajaAgarrada;
    public Animator anim;
    private Collider2D boxCollider;
    private Collider2D playerCollider;


    // Start is called before the first frame update
    void Start()
    {
        druidaMovement = FindAnyObjectByType<FVDruida>();
        bearMovement = FindAnyObjectByType<BearMovement>();
        characterManager = FindObjectOfType<CharacterManager>();
        playerCollider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        DetectBox();
        DetectWall();
        
    }

    private void DetectBox()
    {
        
        //Raycast para detectar si hay una caja que coger hacia donde miramos
        if(characterManager.n == 0)
        {
            lookDirection = druidaMovement.lookDirection;
            distanciaAgarrar = 0.8;
            distanciaPared = 1;
        }else if(characterManager.n == 1)
        {
            lookDirection = bearMovement.lookDirection;
            distanciaAgarrar = 1.1;
            distanciaPared = 2;
        }
        RaycastHit2D hitBox = Physics2D.Raycast(transform.position, lookDirection, (float)distanciaAgarrar, cajaLayer);
        Debug.DrawRay(transform.position, lookDirection * (float)distanciaAgarrar, Color.green);

        //Al presionar E
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(agarrado)
            {
                if(characterManager.n == 0)
                {
                    if(druidaMovement.onFloor)
                    {
                        Debug.DrawRay(transform.position, lookDirection * (float)distanciaPared, Color.red);

                        Debug.Log("Soltando caja");
                        agarrado = false;
                        if (cajaAgarrada != null)
                        {
                            cajaAgarrada.parent = null;
                            cajaAgarrada = null;
                        }
                        anim.SetBool("BoxGrab", false);
                        Physics2D.IgnoreCollision(playerCollider, boxCollider, false);
                    }
            }else if(characterManager.n == 1)
                {
                    Debug.Log("Soltando caja");
                    agarrado = false;
                    if (cajaAgarrada != null)
                    {
                        cajaAgarrada.parent = null;
                        cajaAgarrada = null;
                    }
                    anim.SetBool("BoxGrab", false);
                    Physics2D.IgnoreCollision(playerCollider, boxCollider, false);
                }
                

            } 
            else if (hitBox.collider != null)
            {
                Debug.Log("Agarrando caja");
                agarrado = true;
                GameObject caja = hitBox.collider.gameObject;
                caja.transform.parent = this.transform;
                cajaAgarrada = caja.transform;
                boxCollider = cajaAgarrada.GetComponent<Collider2D>();

                Physics2D.IgnoreCollision(playerCollider, boxCollider, true);

                anim.SetBool("BoxGrab", true);
                anim.SetFloat("MoveX", anim.GetFloat("LastX"));
                anim.SetFloat("MoveY", anim.GetFloat("LastY"));

            }
        }
    }

    private void DetectWall()
    {
        if(cajaAgarrada !=null)
        {
            RaycastHit2D hitWall = Physics2D.Raycast(cajaAgarrada.position, lookDirection, (float)distanciaPared / 2, paredLayer);
            if(hitWall.collider != null)
            {
                paredDelante = true;
            }
            else
            {
                paredDelante = false;
            }
        }
    }
}
