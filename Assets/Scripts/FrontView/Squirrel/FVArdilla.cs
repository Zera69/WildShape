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
    
}