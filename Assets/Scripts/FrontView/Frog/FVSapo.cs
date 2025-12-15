using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FVSapo : MonoBehaviour
{
    public float velocidad = 3f;
    public float fuerzaSalto = 15f;
   

    private Rigidbody2D rb;
    private Animator anim;
    public bool onFloor;
    public FVHook ScriptHook;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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
            onFloor = true;
            anim.SetBool("IsJumping", false);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            onFloor = false;
            anim.SetBool("IsJumping", true);
        }
    }

    void Movement()
    {
        float movX = Input.GetAxis("Horizontal");
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

        if (Input.GetKeyDown(KeyCode.Space) && onFloor)
        {
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
        }

    }
    
}
