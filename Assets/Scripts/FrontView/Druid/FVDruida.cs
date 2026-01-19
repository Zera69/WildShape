using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FVDruida : MonoBehaviour
{ 
    public float velocidad = 15f;
    private float fuerzaSalto = 11f;

    private Rigidbody2D rb;
    private Animator anim;
    public bool onFloor;
    public LayerMask WallsLayer;
    public LayerMask FloorLayer;
    public bool wallRight;
    public bool wallLeft;

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
        if (Input.GetKeyDown(KeyCode.Space) && onFloor)
        {
            Jump();
        }
        CheckFloor();
        CheckWall();

        float movX = Input.GetAxis("Horizontal");
        if (wallRight && !onFloor)
        {
            if (movX > 0)
            {
                movX = 0;
            }
        }
        if (wallLeft && !onFloor)
        {
            if (movX < 0)
            {
                movX = 0;
            }
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

    private void CheckWall()
    {
        Vector2 offsetDown = Vector2.down * 0.45f;
        Vector2 offsetUp = Vector2.up * 0.45f;
        RaycastHit2D hitRightDown = Physics2D.Raycast( (Vector2)transform.position + offsetDown, Vector2.right, 0.6f, WallsLayer);
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

    private void CheckFloor()
    {
        //Raycast Derecha
        Vector2 originRight = transform.position + Vector3.right * 0.5f;
        RaycastHit2D hitFloor = Physics2D.Raycast(originRight, Vector2.down, 0.7f, FloorLayer);
        Debug.DrawRay(originRight, Vector2.down * 0.7f, Color.blue);

        //Raycast Izquierda
        Vector2 originLeft = transform.position + Vector3.left * 0.5f;
        RaycastHit2D hitFloorLeft = Physics2D.Raycast(originLeft, Vector2.down, 0.7f, FloorLayer);
        Debug.DrawRay(originLeft, Vector2.down * 0.7f, Color.blue);

        //Raycast Centro
        Vector2 originCenter = transform.position;
        RaycastHit2D hitFloorCenter = Physics2D.Raycast(originCenter, Vector2.down, 0.7f, FloorLayer);
        Debug.DrawRay(originCenter, Vector2.down * 0.7f, Color.blue);

        if (hitFloor.collider != null || hitFloorLeft.collider != null || hitFloorCenter.collider != null)
        {
            onFloor = true;
        }
        else
        {
            onFloor = false;
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
    }

}
