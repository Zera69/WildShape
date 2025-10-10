using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FVSapo : MonoBehaviour
{
    public float velocidad = 4f;
    public float fuerzaSalto = 15f;
   

    private Rigidbody2D rb;
    private bool enSuelo;
    public FVHook ScriptHook;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(!ScriptHook.isHooked)
        {
            Movement();
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

    void Movement()
    {
        float movX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(movX * velocidad, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
        }

    }
    
}
