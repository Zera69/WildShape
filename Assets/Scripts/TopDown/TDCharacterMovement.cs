using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDCharacterMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float checkDistance = 1f;
    public float gridSize = 1f;
    public float limit = .05f;
    public Transform movePoint;
    public LayerMask stopColliders;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        movePoint.position = transform.position;
        anim = GetComponent<Animator>();
        anim.SetFloat("LastX", 0);
        anim.SetFloat("LastY", -1);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, movePoint.position) <= limit)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == gridSize)
            {
                anim.SetBool("IsMoving", true);

                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    anim.SetFloat("LastX", 1);
                    anim.SetFloat("LastY", 0);
                } else if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    anim.SetFloat("LastX", -1);
                    anim.SetFloat("LastY", 0);
                }
                
                RaycastHit2D hit = Physics2D.Raycast(movePoint.position, new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), checkDistance, stopColliders);
                if (hit.collider==null)
                {
                    
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                }
                
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == gridSize)
            {
                anim.SetBool("IsMoving", true);

                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    anim.SetFloat("LastX", 0);
                    anim.SetFloat("LastY", 1);
                }
                else if (Input.GetAxisRaw("Vertical") < 0)
                {
                    anim.SetFloat("LastX", 0);
                    anim.SetFloat("LastY", -1);
                }

                RaycastHit2D hit = Physics2D.Raycast(movePoint.position, new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), checkDistance, stopColliders);
                if (hit.collider==null)
                {
                    
                    movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                }
            }
            else
            {
                anim.SetBool("IsMoving", false);
            }
        } 

    }
}
