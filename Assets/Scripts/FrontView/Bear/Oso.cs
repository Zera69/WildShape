using UnityEngine;

public class Oso : MonoBehaviour
{
    public float velocidadNormal = 5f;
    public float velocidadArrastrando = 2.5f;
    public KeyCode teclaAgarrar = KeyCode.E;
    
    private Rigidbody2D rbOso;
    private GameObject cajaArrastrada;
    private bool arrastrando = false;
    
    void Start()
    {
        rbOso = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        float movX = Input.GetAxis("Horizontal");
        float velocidad = arrastrando ? velocidadArrastrando : velocidadNormal;
        rbOso.velocity = new Vector2(movX * velocidad, rbOso.velocity.y);
        
        if (Input.GetKeyDown(teclaAgarrar))
        {
            if (!arrastrando)
            {
                BuscarYCogerCaja();
            }
            else
            {
                SoltarCaja();
            }
        }
    }
    
    void BuscarYCogerCaja()
    {
        // Buscar todas las cajas en un radio pequeño
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1.5f);
        
        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("MetalBox"))
            {
                AgarrarCaja(col.gameObject);
                return;
            }
        }
    }
    
    void AgarrarCaja(GameObject caja)
    {
        cajaArrastrada = caja;
        arrastrando = true;
        //hacemos que la caja sea hija del oso
        cajaArrastrada.transform.SetParent(transform);
        
        //caja al lado del oso
        float direccion = transform.localScale.x;
        cajaArrastrada.transform.localPosition = new Vector3(0.8f * direccion, 0.2f, 0);
        
        // quitamos fisica de la caja completamente
        Rigidbody2D rbCaja = cajaArrastrada.GetComponent<Rigidbody2D>();
        if (rbCaja != null)
        {
            rbCaja.simulated = false; // simulated a false hace que ya no se vea afectada por las fisicas
        }
    }
    
    void SoltarCaja()
    {
        if (cajaArrastrada == null) return;
        
        // hacemos que ya no sea hija
        cajaArrastrada.transform.SetParent(null);
        
        // ponemos simulated a true
        Rigidbody2D rbCaja = cajaArrastrada.GetComponent<Rigidbody2D>();
        if (rbCaja != null)
        {
            rbCaja.simulated = true;
            rbCaja.velocity = rbOso.velocity * 0.5f; //pequenyo impulsito
        }
        
        cajaArrastrada = null;
        arrastrando = false;
    }
}