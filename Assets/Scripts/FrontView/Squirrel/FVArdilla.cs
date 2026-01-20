using UnityEngine;

public class FVArdilla : MonoBehaviour
{
    public float velocidad = 40f;
    public float fuerzaSalto = 25f;
    public float gravedadNormal = 20f;
    public float gravedadPlaneo = 0.25f;
    //los numeros muy puestos a ojo puro seguramente toque cambiarlos
    
    private Rigidbody2D rb;
    public bool enSuelo = false;

    private Animator anim;
    public LayerMask WallsLayer;
    public LayerMask FloorLayer;
    private bool wallRight;
    private bool wallLeft;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.SetFloat("LastX", 1);
    }

    void Update()
    {
        CheckFloor();
        CheckWall();

        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            Jump();
        }
        
        if (!enSuelo && rb.velocity.y < 0) //si no estoy en el suelo y velocidad de "Y" es negativa cambio la gravedad
        {
            rb.gravityScale = gravedadPlaneo;
            anim.SetBool("IsFloating", true);
        }
        else
        {
            rb.gravityScale = gravedadNormal;
            anim.SetBool("IsFloating", false);
        }
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
        anim.SetBool("IsJumping", true);
    }

    void Movement()
    {
        float movX = Input.GetAxis("Horizontal");
        if(wallRight && !enSuelo)
        {
            if(movX > 0)
            {
                movX = 0;
            }
        }

        if(wallLeft && !enSuelo)
        {
            if(movX < 0)
            {
                movX = 0;
            }
        }

        rb.velocity = new Vector2(movX * velocidad, rb.velocity.y);

        if (movX > 0)
        {
            anim.SetFloat("LastX", 1);
        } else if (movX < 0)
        {
            anim.SetFloat("LastX", 0);
        }
        if (movX == 0)
        {
            anim.SetBool("IsMoving", false);
        }
        else
        {
            anim.SetBool("IsMoving", true);
        }
    }

    private void CheckFloor()
    {
        //Raycast Derecha
        Vector2 originRight = transform.position + Vector3.right * 0.5f;
        RaycastHit2D hitFloor = Physics2D.Raycast(originRight, Vector2.down, 0.3f, FloorLayer);
        Debug.DrawRay(originRight, Vector2.down * 0.3f, Color.blue);

        //Raycast Izquierda
        Vector2 originLeft = transform.position + Vector3.left * 0.5f;
        RaycastHit2D hitFloorLeft = Physics2D.Raycast(originLeft, Vector2.down, 0.3f, FloorLayer);
        Debug.DrawRay(originLeft, Vector2.down * 0.3f, Color.blue);

        //Raycast Centro
        Vector2 originCenter = transform.position;
        RaycastHit2D hitFloorCenter = Physics2D.Raycast(originCenter, Vector2.down, 0.3f, FloorLayer);
        Debug.DrawRay(originCenter, Vector2.down * 0.3f, Color.blue);
        
        if (hitFloor.collider != null || hitFloorLeft.collider != null || hitFloorCenter.collider != null)
        {
            enSuelo = true;
            velocidad = 7;
            anim.SetBool("IsJumping", false);
        }
        else
        {
            enSuelo = false;
            velocidad = 4;
            anim.SetBool("IsJumping", true);
        }
    }

    private void CheckWall()
    {
        Vector2 offsetDown = Vector2.down * 0.45f;
        Vector2 offsetUp = Vector2.up * 0.45f;
        RaycastHit2D hitRightDown = Physics2D.Raycast((Vector2)transform.position + offsetDown, Vector2.right, 0.6f, WallsLayer);
        RaycastHit2D hitLeftDown = Physics2D.Raycast((Vector2)transform.position + offsetDown, Vector2.left, 0.6f, WallsLayer);

        Debug.DrawRay((Vector2)transform.position + offsetDown, Vector2.right * 0.6f, Color.green);
        Debug.DrawRay((Vector2)transform.position + offsetDown, Vector2.left * 0.6f, Color.green);

        RaycastHit2D hitRightUp = Physics2D.Raycast((Vector2)transform.position + offsetUp, Vector2.right, 0.6f, WallsLayer);
        RaycastHit2D hitLeftUp = Physics2D.Raycast((Vector2)transform.position + offsetUp, Vector2.left, 0.6f, WallsLayer);

        Debug.DrawRay((Vector2)transform.position + offsetUp, Vector2.right * 0.6f, Color.green);
        Debug.DrawRay((Vector2)transform.position + offsetUp, Vector2.left * 0.6f, Color.green);

        if (hitRightUp.collider != null || hitRightDown.collider != null)
        {
            wallRight = true;
        }
        else
        {
            wallRight = false;
        }

        if (hitLeftUp.collider != null || hitLeftDown.collider != null)
        {
            wallLeft = true;
        }
        else
        {
            wallLeft = false;
        }
    }     
    
}