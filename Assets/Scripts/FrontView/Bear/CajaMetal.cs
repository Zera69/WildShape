using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CajaMetal : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool siendoEmpujada = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Oso"))
        {
            siendoEmpujada = true;
            
            // libera solo x cuando el oso toca la caja
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | 
                            RigidbodyConstraints2D.FreezeRotation;//no he encontrado un unfreeze o algo asi
        }
    }
    
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Oso"))
        {
            siendoEmpujada = false;
            
            // congelar todo cuando el oso se aleja
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            rb.velocity = Vector2.zero; // detener cualquier movimiento residual
        }
    }
}
