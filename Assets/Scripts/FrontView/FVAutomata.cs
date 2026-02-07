using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FVAutomata : MonoBehaviour
{

    private Animator anim;
    public Vector2 lookDirection;
    public bool IsMoving = false;
    public int moveSpeed = 1;

    public Transform objetivo;
    private bool mover = false;

    public string animal;
    private SaveData sd;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetFloat("LastX", -1);
        lookDirection = Vector2.left;
        sd = SaveManager.instance.GetData();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            //Debug.Log("oso");
            anim.SetFloat("LastX", 1);
            anim.SetBool("IsMoving", true);
            mover = true;
            sd.unlockedCharacters.Add(animal);
            SaveManager.instance.SaveGame();
        }
        
    }

    void Move()
    {
        if (mover)
        {
            transform.position = Vector3.MoveTowards(transform.position, objetivo.position, moveSpeed * Time.deltaTime);
        }
    }

}
