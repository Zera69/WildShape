using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FVSapo : MonoBehaviour
{
    public float velocidad = 3f;
    public float fuerzaSalto = 15f;
   

    public Rigidbody2D rb;
    private Animator anim;
    public bool onFloor;
    public FVHook ScriptHook;
    public LayerMask WallsLayer;
    public LayerMask FloorLayer;
    public bool wallRight;
    public bool wallLeft;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && onFloor)
        {
            Jump();
        }
        ChechFloor();
        CheckWall();
        
    }

    void FixedUpdate()
    {
        if(!ScriptHook.isHooked)
        {
            Movement();
        }
    }
    

    private void CheckWall()
    {
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, 0.6f, WallsLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 0.6f, WallsLayer);

        Debug.DrawRay(transform.position, Vector2.right * 0.6f, Color.red);
        Debug.DrawRay(transform.position, Vector2.left * 0.6f, Color.red);

        if (hitRight.collider != null )
        {
            wallRight = true;
        }
        else
        {
            wallRight = false;
        }

        if (hitLeft.collider != null)
        {
            wallLeft = true;
        }
        else
        {
            wallLeft = false;
        }
    } 

    private void ChechFloor()
    {
        //Raycast Derecha
        Vector2 originRight = transform.position + Vector3.right * 0.5f;
        RaycastHit2D hitFloor = Physics2D.Raycast(originRight, Vector2.down, 0.7f, FloorLayer);
        Debug.DrawRay(originRight, Vector2.down * 0.7f, Color.blue);

        //Raycast Izquierda
        Vector2 originLeft = transform.position + Vector3.left * 0.5f;
        RaycastHit2D hitFloorLeft = Physics2D.Raycast(originLeft, Vector2.down, 0.7f, FloorLayer);
        Debug.DrawRay(originLeft, Vector2.down * 0.7f, Color.blue);

        //Raycast Centro
        Vector2 originCenter = transform.position;
        RaycastHit2D hitFloorCenter = Physics2D.Raycast(originCenter, Vector2.down, 0.7f, FloorLayer);
        Debug.DrawRay(originCenter, Vector2.down * 0.7f, Color.blue);
        
        if (hitFloor.collider != null || hitFloorLeft.collider != null || hitFloorCenter.collider != null)
        {
            onFloor = true;
        }
        else
        {
            onFloor = false;
        }
    }

    void Movement()
    {
        float movX = Input.GetAxis("Horizontal");
        if(wallRight && !onFloor)
        {
            if(movX > 0)
            {
                movX = 0;
            }
        }
        if(wallLeft && !onFloor)
        {
            if(movX < 0)
            {
                movX = 0;
            }
        }
        rb.velocity = new Vector2(movX * velocidad, rb.velocity.y);

        if(movX == 0)
        {
            anim.SetBool("IsMoving", false);
        } else
        {
            anim.SetBool("IsMoving", true);
        }

        if (movX > 0)
        {
            anim.SetFloat("LastX", 1);
        }
        else if (movX < 0)
        {
            anim.SetFloat("LastX", 0);
        }

    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
    }
    
}
