using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FVHook : MonoBehaviour
{

    private float forceInHook = 7f;

    public float inputHorizontal;
    public float inputVertical;

    public Rigidbody2D rb;

    private bool isHooked = false;
    private bool isPulling = false;
    private bool PresingClick;

    public DistanceJoint2D dj;
    private DistanceJoint2D djPull;

    public LayerMask hookableLayer;
    public LayerMask pullableLayer;

    private GameObject hookPoint;
    private GameObject pullPoint;
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
        HookAndPull();
        HandleInput();
        DisableScript();
        DetectPresing();
    }

    void FixedUpdate()
    {
        MoveOnHook();
    }

    void HookAndPull()
    {
        //Miramos donde esta la posicion del raton
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        //Miramos neustra posicion y calculamos la direccion hacia el raton
        Vector2 origin = transform.position;
        Vector2 direction = (mouseWorldPos - transform.position).normalized;

        //Lanzmaos un raycast desde nosotros hacia el raton
        RaycastHit2D hitHook = Physics2D.Raycast(origin, direction, 3f, hookableLayer);
        RaycastHit2D hitPull = Physics2D.Raycast(origin, direction, 3f, pullableLayer);

        //Lo dibujamos en pantalla
        Debug.DrawRay(origin, direction * 3f, Color.blue);

        //Detectamos si hay algo que hookear a nuestro alcance
        if (hitHook.collider != null)
        {
            hookPoint = hitHook.collider.gameObject;
        }

        //Detectamos si hay algo que pullear de nuestro alcance
        if (hitPull.collider != null)
        {
            pullPoint = hitPull.collider.gameObject;
        }


        //Si hacmeos click izquierda, no estamos cogidos y detectamos donde cogernos entramos en el if
        if (PresingClick && !isHooked && hitHook.collider != null)
        {
            //activamos la conexion
            dj.enabled = true;
            dj.connectedBody = hitHook.collider.GetComponent<Rigidbody2D>();
            isHooked = true;

        }
        else if (!PresingClick && isHooked)
        {
            //descactivamos la conexion
            dj.enabled = false;
            dj.connectedBody = null;
            isHooked = false;
        }

        if (Input.GetKeyDown(KeyCode.E) && hitPull.collider != null && !isPulling)
        {

            if (djPull == null)
            {
                djPull = pullPoint.AddComponent<DistanceJoint2D>();
            }
            djPull.enabled = true;
            djPull.connectedBody = rb;
            isPulling = true;

        }
        else if (Input.GetKeyDown(KeyCode.E) && isPulling)
        {
            Destroy(djPull);
            isPulling = false;
            djPull.enabled = false;
            djPull.connectedBody = null;
        }

        if (isPulling && PresingClick)
        {
            djPull.distance -= 2f * Time.deltaTime;
            djPull.distance = Mathf.Clamp(djPull.distance, 0.8f, 3f);
            
        }


    }

    void MoveOnHook()
    {
        // Añadir fuerza horizontal al player  
        if (isHooked)
        {
            Vector2 force = new Vector2(inputHorizontal * forceInHook, 0);
            rb.AddForce(force);

            //Acercar y alejar el gancho con las teclas verticales
            dj.distance -= inputVertical * 2f * Time.deltaTime;
            //Limitar la distancia minima y maxima del gancho
            dj.distance = Mathf.Clamp(dj.distance, 0.2f, 2.7f);
            
            

        }
    }

    void PullObject()
    {
        if (isPulling)
        {
            //pass
        }
    }

    
    /*No creo que se use por ahora
    void ImpulseOnHook()
    {
        Vector2 force = new Vector2(forceImpulseOnHook, 0);
        rb.AddForce(force, ForceMode2D.Impulse);
    }
    */
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

    void DetectPresing()
    {
        if (Input.GetMouseButton(0))
        {
            PresingClick = true;
            rb.gravityScale = 4f;
        }
        else
        {
            PresingClick = false;
        }
    }
}
