using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgarrarCaja : MonoBehaviour
{
    public bool agarrado = false;
    public LayerMask cajaLayer;
    public  LayerMask paredLayer;
    public double distanciaAgarrar = 1;
    public double distanciaPared = 0.5;
    private TDCharacterMovement characterMovement;
    private TDCharacterManager characterManager;
    private Vector2 lookDirection;
    public bool paredDelante = false;
    public Transform cajaAgarrada;
    public Animator anim;

    //Druida
    private RaycastHit2D hitBox;
    //oso
    private Vector2 offsetUp;
    private Vector2 offsetDown;
    private RaycastHit2D hitBear;
    private RaycastHit2D hitBear2;
    private Vector2 offsetRight;
    private Vector2 offsetLeft;

    // Start is called before the first frame update
    void Start()
    {
        characterMovement = FindAnyObjectByType<TDCharacterMovement>();
        characterManager = FindObjectOfType<TDCharacterManager>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        DetectBox();
    }

    private void DetectBox()
    {
        //Miramos hacia donde mira el personaje
        lookDirection = characterMovement.lookDirection;
        //Raycast para detectar si hay una caja que coger hacia donde miramos
        if(characterManager.n == 0)
        {
            distanciaAgarrar = 1;
            distanciaPared = 1;
        }else if(characterManager.n == 1)
        {
            distanciaAgarrar = 1.5;
            distanciaPared = 1.5;
        }

        if(characterManager.n == 1) // Si es oso, hacemos raycasts adicionales para el tamaño
        {
                if(characterMovement.moveDir.x != 0 && !agarrado)
                {
                    offsetUp = Vector2.up * 0.5f;
                    offsetDown = Vector2.down * 0.5f;
                    hitBear = Physics2D.Raycast((Vector2)transform.position + offsetUp, lookDirection, (float)distanciaAgarrar, cajaLayer);
                    hitBear2 = Physics2D.Raycast((Vector2)transform.position + offsetDown, lookDirection, (float)distanciaAgarrar, cajaLayer);  
                }
                else if(characterMovement.moveDir.y != 0 && !agarrado)
                {
                    offsetRight = Vector2.right * 0.5f;
                    offsetLeft = Vector2.left * 0.5f;
                    hitBear = Physics2D.Raycast((Vector2)transform.position + offsetRight, lookDirection, (float)distanciaAgarrar, cajaLayer);
                    hitBear2 = Physics2D.Raycast((Vector2)transform.position + offsetLeft, lookDirection, (float)distanciaAgarrar, cajaLayer);
                }
                Debug.DrawRay((Vector2)transform.position + offsetRight, lookDirection * (float)distanciaAgarrar, Color.green);
                Debug.DrawRay((Vector2)transform.position + offsetLeft, lookDirection * (float)distanciaAgarrar, Color.green);
                Debug.DrawRay((Vector2)transform.position + offsetUp, lookDirection * (float)distanciaAgarrar, Color.green);
                Debug.DrawRay((Vector2)transform.position + offsetDown, lookDirection * (float)distanciaAgarrar, Color.green);
                
        }else if (characterManager.n == 0 )
        {
            hitBox = Physics2D.Raycast(transform.position, lookDirection, (float)distanciaAgarrar, cajaLayer);
            Debug.DrawRay(transform.position, lookDirection * (float)distanciaAgarrar, Color.green); 
        }

        //Al presionar E
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(characterManager.n == 0 )
            {
                if(agarrado && characterMovement.movePoint.position == transform.position)
                {
                    agarrado = false;
                    cajaAgarrada.parent = null;
                    cajaAgarrada = null;

                    anim.SetBool("BoxGrab", false);

                } 
                else if (hitBox.collider != null && characterMovement.movePoint.position == transform.position)
                {
                    agarrado = true;
                    GameObject caja = hitBox.collider.gameObject;
                    caja.transform.parent = this.transform;
                    cajaAgarrada = caja.transform;

                    anim.SetBool("BoxGrab", true);
                    anim.SetFloat("MoveX", anim.GetFloat("LastX"));
                    anim.SetFloat("MoveY", anim.GetFloat("LastY"));

                }
            }else if(characterManager.n == 1)
            {
                if(agarrado && characterMovement.movePoint.position == transform.position)
                {
                    agarrado = false;
                    cajaAgarrada.parent = null;
                    cajaAgarrada = null;

                    anim.SetBool("BoxGrab", false);

                } 
                else if (hitBear.collider!=null && hitBear2.collider!=null && characterMovement.movePoint.position == transform.position)
                {
                    agarrado = true;
                    GameObject caja = hitBear.collider.gameObject;
                    caja.transform.parent = this.transform;
                    cajaAgarrada = caja.transform;

                    anim.SetBool("BoxGrab", true);
                    anim.SetFloat("MoveX", anim.GetFloat("LastX"));
                    anim.SetFloat("MoveY", anim.GetFloat("LastY"));

                }
            }
            
        }
    }

    public bool DetectWallDruida()
    {
        Vector2 dir = characterMovement.moveDir;
        Vector2 origin = cajaAgarrada.position;

        RaycastHit2D[] hitWall = Physics2D.RaycastAll(origin, dir, (float)distanciaPared, paredLayer);
        if(hitWall.Length > 1)
        {
            paredDelante = true;
        }
        else
        {
            paredDelante = false;
        }

        return paredDelante;
    }

    public bool DetectWallBear()
    {
        Vector2 dir = characterMovement.moveDir;
        Vector2 origin = cajaAgarrada.position;

        Vector2 offsetA;
        Vector2 offsetB;

        //Si nos movemos en X, hacemos los raycast un poco arriba y abajo
        //Si nos movemos en Y, hacemos los raycast un poco a la izquierda y derecha
        if (Mathf.Abs(dir.x) > 0)
        {
            offsetA = Vector2.up * 0.5f;
            offsetB = Vector2.down * 0.5f;
            RaycastHit2D[] hitWallA = Physics2D.RaycastAll((Vector2)origin + offsetA, dir, (float)distanciaPared, paredLayer);
            RaycastHit2D[] hitWallB = Physics2D.RaycastAll((Vector2)origin + offsetB, dir, (float)distanciaPared, paredLayer);
            if (hitWallA.Length > 1 || hitWallB.Length > 1)
            {
                paredDelante = true;
            }
            else
            {
                paredDelante = false;
            }
        }
        else
        {
            offsetA = Vector2.right * 0.5f;
            offsetB = Vector2.left * 0.5f;
            RaycastHit2D[] hitWallA = Physics2D.RaycastAll((Vector2)origin + offsetA, dir, (float)distanciaPared, paredLayer);
            RaycastHit2D[] hitWallB = Physics2D.RaycastAll((Vector2)origin + offsetB, dir, (float)distanciaPared, paredLayer);
            if (hitWallA.Length > 1 || hitWallB.Length > 1)
            {
                paredDelante = true;
            }
            else
            {
                paredDelante = false;
            }
        }
        return paredDelante;

    }
}
