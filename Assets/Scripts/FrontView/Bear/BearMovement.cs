using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearMovement : MonoBehaviour
{ 
    public float velocidad = 15f;
    private float fuerzaSalto = 11f;

    public Rigidbody2D rb;
    private Animator anim;
    public Vector2 lookDirection = new Vector2(1, 0);
    private Agarrar agarrarScript;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        agarrarScript = GetComponent<Agarrar>();
        anim.SetFloat("LastX", 1);
    }

    // Update is called once per frame
    void Update()
    {
       
        float movX = Input.GetAxis("Horizontal");
        if (agarrarScript.agarrado && agarrarScript.paredDelante)
        {
            if (Mathf.Sign(movX) == Mathf.Sign(lookDirection.x))
            {
                movX = 0;
            }
        }
        if(movX != 0)
        {
            lookDirection = new Vector2(movX, 0);
        }
        rb.velocity = new Vector2(movX * velocidad, rb.velocity.y);

        if (movX > 0)
        {
            anim.SetFloat("LastX", 1);
        }
        else if (movX < 0)
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


}
