using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FVHook : MonoBehaviour
{

    private float forceInHook = 5f;
    private float forceImpulseOnHook = 70f;
    public float inputHorizontal;
    public float inputVertical;
    public Rigidbody2D rb;
    private bool isHooked = false;
    public DistanceJoint2D dj;
    public LayerMask hookableLayer;
    private GameObject hookPoint;
    public GameObject spawnRope;
    public Camera cam;
    public FVArdilla ScriptArdilla;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Hook();
        MoveOnHook();
        HandleInput();
        DisableScript();
    }

    void Hook()
    {
        //Miramos donde esta la posicion del raton
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        //Miramos neustra posicion y calculamos la direccion hacia el raton
        Vector2 origin = transform.position;
        Vector2 direction = (mouseWorldPos - transform.position).normalized;

        //Lanzmaos un rascast desde nosotros hacia el raton
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, 3f, hookableLayer);

        //Lo dibujamos en pantalla
        Debug.DrawRay(origin, direction * 3f, Color.blue);
        if (hit.collider != null)
        {
            hookPoint = hit.collider.gameObject;
        }


        //Si hacmeos click izquierda, no estamos cogidos y detectamos donde cogernos entramos en el if
        if (Input.GetMouseButtonDown(0) && !isHooked && hit.collider != null)
        {
            //activamos la conexion
            dj.enabled = true;
            dj.connectedBody = hit.collider.GetComponent<Rigidbody2D>();
            isHooked = true;
            forceImpulseOnHook = 30f;
            ImpulseOnHook();
        }
        else if (Input.GetMouseButtonDown(0) && isHooked)
        {
            //descactivamos la conexion
            dj.enabled = false;
            dj.connectedBody = null;
            isHooked = false;
            forceImpulseOnHook = 30f;
            ImpulseOnHook();
        }


    }

    void MoveOnHook()
    {
        // Añadir fuerza horizontal al player  
        if (isHooked)
        {
            Vector2 force = new Vector2(inputHorizontal * forceInHook, 0);
            rb.AddForce(force);

            dj.distance -= inputVertical * 2f * Time.deltaTime;
        }
    }

    void ImpulseOnHook()
    {
        Vector2 force = new Vector2(forceImpulseOnHook, 0);
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    void HandleInput()
    {
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");
    }

    void DisableScript()
    {
        if (isHooked)
        {
            ScriptArdilla.enabled = false;
        }
        else if (!isHooked)
        {
            ScriptArdilla.enabled = true;
        }
    }

}
