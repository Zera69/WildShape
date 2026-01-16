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
    private Vector2 lookDirection;
    public bool paredDelante = false;
    public Transform cajaAgarrada;
    // Start is called before the first frame update
    void Start()
    {
        characterMovement = GetComponent<TDCharacterMovement>();
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
        RaycastHit2D hitBox = Physics2D.Raycast(transform.position, lookDirection, (float)distanciaAgarrar, cajaLayer);

        //Al presionar E
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(agarrado)
            {
                agarrado = false;
                Transform caja = this.transform.GetChild(0);
                caja.parent = null;
                Debug.Log("Caja soltada");
            } 
            else if (hitBox.collider != null)
            {
                agarrado = true;
                GameObject caja = hitBox.collider.gameObject;
                caja.transform.parent = this.transform;
                cajaAgarrada = caja.transform;
                Debug.Log("Caja agarrada");
            
            }
        }
    }
}
