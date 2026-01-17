using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDCharacterMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float checkDistance = 1f;
    public float gridSize = 1f;
    public float limit = .001f;
    public Transform movePoint;
    public LayerMask stopColliders;


    public Vector2 lookDirection;
    private AgarrarCaja agarrarCaja;
    private float Horizontal;
    private float Vertical;
    private TDCharacterManager characterManager;
    private RaycastHit2D hitWall;


    private Animator anim;
    public Vector2 moveDir;
    public bool IsMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        characterManager = FindObjectOfType<TDCharacterManager>();
        movePoint.position = transform.position;
        anim = GetComponent<Animator>();
        anim.SetFloat("LastX", 0);
        anim.SetFloat("LastY", -1);
        lookDirection = Vector2.down;
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, movePoint.position) > 0f)
        {
            IsMoving = true;
        }
        else
        {
            IsMoving = false;
        }
        if (characterManager.n == 0)
        {
            agarrarCaja = FindObjectOfType<AgarrarCaja>();
        }
        DetectInputs();
    }

    void FixedUpdate()
    {
        Move();
    }

    void DetectInputs()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");
    }

    void Move()
    {
        // Mueve suavemente hacia el movePoint
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        // Si estamos cerca del movePoint, podemos movernos de nuevo
        if (Vector3.Distance(transform.position, movePoint.position) <= limit)
        {
            // Determina la dirección de movimiento
            moveDir = Vector2.zero;

            //Detectamos si nos movemos en (1,0) o (-1,0)
            if (Mathf.Abs(Horizontal) == gridSize)
            {
                moveDir = new Vector2(Horizontal, 0);
            }

            //Detectamos si nos movemos en (0,1) o (0,-1)
            if (Mathf.Abs(Vertical) == gridSize)
            {
                moveDir = new Vector2(0, Vertical);
            }

            bool canMove = moveDir != Vector2.zero;

            // Actualiza lookDirection
            if (canMove)
            {
                lookDirection = moveDir;
            }

            // Si hay caja agarrada y somos druida, comprobamos si hay pared delante de la caja
            if (canMove && agarrarCaja!=null && characterManager.n == 0 && agarrarCaja.agarrado && agarrarCaja.cajaAgarrada != null)
            {
                //Lanzamos raycast desde la caja hacia donde se quiere mover el jugador desde la caja
                 hitWall = Physics2D.Raycast(agarrarCaja.cajaAgarrada.position,moveDir,(float)agarrarCaja.distanciaPared,agarrarCaja.paredLayer);
                 Debug.DrawRay(agarrarCaja.cajaAgarrada.position, moveDir * (float)agarrarCaja.distanciaPared, Color.blue);

                //Si hay una pared delante, no se puede mover
                if (hitWall.collider != null)
                {
                    canMove = false;
                }

            }
            
            // Comprobamos si hay colisionadores que bloqueen el movimiento
            if (canMove)
            {
                if(characterManager.n == 1) // Si es oso, usamos boxcast
                {
                    if(moveDir.x != 0)
                    {
                        Vector2 offsetUp = Vector2.up * 0.5f;
                        Vector2 offsetDown = Vector2.down * 0.5f;
                        RaycastHit2D hitBear = Physics2D.Raycast((Vector2)movePoint.position + offsetUp, moveDir, checkDistance, stopColliders);
                        RaycastHit2D hitBear2 = Physics2D.Raycast((Vector2)movePoint.position + offsetDown, moveDir, checkDistance, stopColliders);
                        Debug.DrawRay((Vector2)movePoint.position + offsetUp, moveDir * checkDistance, Color.red);
                        Debug.DrawRay((Vector2)movePoint.position + offsetDown, moveDir * checkDistance, Color.red);
                        if (hitBear.collider != null || hitBear2.collider != null)
                        {
                            canMove = false;
                        }
                    }else if(moveDir.y != 0)
                    {
                        Vector2 offsetRight = Vector2.right * 0.5f;
                        Vector2 offsetLeft = Vector2.left * 0.5f;
                        RaycastHit2D hitBear = Physics2D.Raycast((Vector2)movePoint.position + offsetRight, moveDir, checkDistance, stopColliders);
                        RaycastHit2D hitBear2 = Physics2D.Raycast((Vector2)movePoint.position + offsetLeft, moveDir, checkDistance, stopColliders);
                        Debug.DrawRay((Vector2)movePoint.position + offsetRight, moveDir * checkDistance, Color.red);
                        Debug.DrawRay((Vector2)movePoint.position + offsetLeft, moveDir * checkDistance, Color.red);
                        if (hitBear.collider != null || hitBear2.collider != null)
                        {
                            canMove = false;
                        }
                    }
                
                }
                else
                {
                    RaycastHit2D hitPlayer = Physics2D.Raycast(movePoint.position, moveDir, checkDistance, stopColliders);
                    Debug.DrawRay(movePoint.position, moveDir * checkDistance, Color.red);
                    if (hitPlayer.collider != null)
                    {
                        canMove = false;
                    }
                }
               
                
                //Si detecta colision
                if (!canMove) 
                {
                    // Si estamos llevando caja
                    if (agarrarCaja != null && agarrarCaja.agarrado && agarrarCaja.cajaAgarrada != null)
                    {
                        RaycastHit2D hitPlayer = Physics2D.Raycast(movePoint.position, moveDir, checkDistance, stopColliders);
                        // Si el collider no es la caja, bloqueamos movimiento
                        if (hitPlayer.collider.gameObject != agarrarCaja.cajaAgarrada.gameObject)
                        {
                            canMove = false;
                        }
                        // Si el collider es la caja, pasamos
                        else
                        {
                            movePoint.position += (Vector3)moveDir; // Collider es la caja -> dejamos pasar
                            canMove = false; // Ya movimos, no repetir
                        }
                    }
                    // Si no estamos llevando caja
                    else
                    {
                        canMove = false; // Collider normal bloquea movimiento
                    }
                }
            }

            // Finalmente, si todos los checks pasaron, movemos
            if (canMove)
            {
                movePoint.position += (Vector3)moveDir;
            }
                
            if(characterManager.n == 0 && agarrarCaja != null && agarrarCaja.agarrado )
            {
                //animacion de llevar caja
            }else
            {
                // Actualizamos animaciones
                anim.SetBool("IsMoving", moveDir != Vector2.zero && canMove);
                if (moveDir != Vector2.zero)
                {
                    anim.SetFloat("LastX", lookDirection.x);
                    anim.SetFloat("LastY", lookDirection.y);
                }
            }
            
        }
    }

}
