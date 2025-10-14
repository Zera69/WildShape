using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FVDruida : MonoBehaviour
{ 
    public float velocidad = 15f;

    private Rigidbody2D rb;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.SetFloat("LastX", 1);
    }

    // Update is called once per frame
    void Update()
    {
        float movX = Input.GetAxis("Horizontal");
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
