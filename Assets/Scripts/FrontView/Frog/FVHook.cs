using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FVHook : MonoBehaviour
{

    private float forceInHook = 20f;
    private float impulseHook = 15f;

    public float inputHorizontal;
    public float inputVertical;

    public Rigidbody2D rb;

    public bool isHooked = false;
    private bool isPulling = false;
    private bool PresingClick;

    public DistanceJoint2D dj;

    public LayerMask hookableLayer;
    public LayerMask pullableLayer;

    private GameObject hookPoint;
    private GameObject pullPoint;
    public GameObject spawnRope;

    public Camera cam;
    public FVSapo ScriptSapo;

    private Animator anim;
    

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HookAndPull();
        HandleInput();
        DetectPresing();
        ChangeMassPulling();
        ChangeMassHooked();


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

        if(direction.x > 0)
        {
            anim.SetFloat("PullX", 1);
        } else if(direction.x < 0)
        {
            anim.SetFloat("PullX", -1);
        }

        //Lanzmaos un raycast desde nosotros hacia el raton
        RaycastHit2D hitHook = Physics2D.Raycast(origin, direction, 4f, hookableLayer);
        RaycastHit2D hitPull = Physics2D.Raycast(origin, direction, 4f, pullableLayer);

        //Lo dibujamos en pantalla
        Debug.DrawRay(origin, direction * 4f, Color.red);

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
            anim.SetBool("TongueOut", true);

        }
        else if (!PresingClick && isHooked)
        {
            //descactivamos la conexion
            dj.enabled = false;
            dj.connectedBody = null;
            ImpulseOnExitHook();
            isHooked = false;
            anim.SetBool("TongueOut", false);

        }

        if (Input.GetKeyDown(KeyCode.E) && hitPull.collider != null && !isPulling)
        {
            //activamos la conexion
            dj.enabled = true;
            dj.connectedBody = hitPull.collider.GetComponent<Rigidbody2D>();
            isPulling = true;
            anim.SetBool("TonguePull", true);

        }
        else if (Input.GetKeyDown(KeyCode.E) && isPulling)
        {
            dj.enabled = false;
            dj.connectedBody = null;
            isPulling = false;
            anim.SetBool("TonguePull", false);
        }

        if (isPulling && PresingClick)
        {
            dj.distance -= 2f * Time.deltaTime;
            dj.distance = Mathf.Clamp(dj.distance, 0.8f, 4.3f);
            
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
            dj.distance = Mathf.Clamp(dj.distance, 0.2f, 3.8f);
            
            

        }
    }

    void ImpulseOnExitHook()
    {
        rb.AddForce(rb.velocity.normalized * impulseHook,ForceMode2D.Impulse);
    }

    void ChangeMassPulling()
    {
        if (isPulling)
        {
            rb.mass = 20;
            ScriptSapo.velocidad = 1f;
            ScriptSapo.fuerzaSalto = 8f;
        }
        else
        {
            rb.mass = 5f;
            ScriptSapo.velocidad = 3f;
            ScriptSapo.fuerzaSalto = 15f;
        }
    }

    void ChangeMassHooked()
    {
        if(isHooked)
        {
            rb.mass = 2;
        }
        else
        {
            rb.mass = 5;
        }
    }

    void HandleInput()
    {
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");
    }

    void DetectPresing()
    {
        if (Input.GetMouseButton(0))
        {
            PresingClick = true;
            
        }
        else
        {
            PresingClick = false;
            
        }
    }
}
