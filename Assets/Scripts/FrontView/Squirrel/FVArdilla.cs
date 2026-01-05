using UnityEngine;

public class FVArdilla : MonoBehaviour
{
    public float velocidad = 40f;
    public float fuerzaSalto = 25f;
    public float gravedadNormal = 20f;
    public float gravedadPlaneo = 0.25f;
    //los numeros muy puestos a ojo puro seguramente toque cambiarlos
    
    private Rigidbody2D rb;
    private bool enSuelo;

    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.SetFloat("LastX", 1);
    }

    void Update()
    {
        float movX = Input.GetAxis("Horizontal");
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

        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
            //anim.SetBool("IsJumping", true);
        }
        
        if (!enSuelo && Input.GetKey(KeyCode.Space) && rb.velocity.y < 0) //si no estoy en el suelo, 
                                                                        //y pulso espacio mientras velocidad de y es negativa (bajo) 
                                                                        // cambio la gravedad
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            enSuelo = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0;
            anim.SetBool("IsJumping", false);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            enSuelo = false;
            anim.SetBool("IsJumping", true);
        }
    }
}