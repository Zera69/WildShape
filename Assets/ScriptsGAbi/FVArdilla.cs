using UnityEngine;

public class ArdillaSimple : MonoBehaviour
{
    public float velocidad = 40f;
    public float fuerzaSalto = 25f;
    public float gravedadNormal = 20f;
    public float gravedadPlaneo = 0.25f;
    //los numeros muy puestos a ojo puro seguramente toque cambiarlos
    
    private Rigidbody2D rb;
    private bool enSuelo;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float movX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(movX * velocidad, rb.velocity.y);
        
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
        }
        
        if (!enSuelo && Input.GetKey(KeyCode.Space) && rb.velocity.y < 0) //si no estoy en el suelo, 
                                                                        //y pulso espacio mientras velocidad de y es negativa (bajo) 
                                                                        // cambio la gravedad
        {
            rb.gravityScale = gravedadPlaneo; 
        }
        else
        {
            rb.gravityScale = gravedadNormal; 
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            enSuelo = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            enSuelo = false;
        }
    }
}