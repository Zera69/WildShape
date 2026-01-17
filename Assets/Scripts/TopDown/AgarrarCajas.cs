using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgarrarCaja : MonoBehaviour
{
    public bool agarrado = false;
    public LayerMask cajaLayer;
    public  LayerMask paredLayer;
    public double distanciaAgarrar = 1;
    public double distanciaPared = 0.5;
    private TDCharacterMovement characterMovement;
    private TDCharacterManager characterManager;
    private Vector2 lookDirection;
    public bool paredDelante = false;
    public Transform cajaAgarrada;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        characterMovement = FindAnyObjectByType<TDCharacterMovement>();
        characterManager = FindObjectOfType<TDCharacterManager>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        DetectBox();
    }

    private void DetectBox()
    {
        //Miramos hacia donde mira el personaje
        lookDirection = characterMovement.lookDirection;
        //Raycast para detectar si hay una caja que coger hacia donde miramos
        if(characterManager.n == 0)
        {
            distanciaAgarrar = 1;
            distanciaPared = 1;
        }else if(characterManager.n == 1)
        {
            distanciaAgarrar = 1.5;
            distanciaPared = 1.5;
        }
        RaycastHit2D hitBox = Physics2D.Raycast(transform.position, lookDirection, (float)distanciaAgarrar, cajaLayer);
        Debug.DrawRay(transform.position, lookDirection * (float)distanciaAgarrar, Color.green);

        //Al presionar E
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(agarrado && characterMovement.movePoint.position == transform.position)
            {
                agarrado = false;
                Transform caja = this.transform.GetChild(0);
                caja.parent = null;

                anim.SetBool("BoxGrab", false);

            } 
            else if (hitBox.collider != null && characterMovement.movePoint.position == transform.position)
            {
                agarrado = true;
                GameObject caja = hitBox.collider.gameObject;
                caja.transform.parent = this.transform;
                cajaAgarrada = caja.transform;

                anim.SetBool("BoxGrab", true);
                anim.SetFloat("MoveX", anim.GetFloat("LastX"));
                anim.SetFloat("MoveY", anim.GetFloat("LastY"));

            }
        }
    }
}
