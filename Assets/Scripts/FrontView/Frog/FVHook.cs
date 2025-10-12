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
    private GameObject drawTongueHookPoint;
    private GameObject drawTonguePullPoint;
    public GameObject spawnRope;

    public Camera cam;
    public FVSapo ScriptSapo;

    private Animator anim;
    public bool canDrawTongue = false;
    

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
        UpdateTongueVisual();
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
        RaycastHit2D hitHook = Physics2D.Raycast(origin, direction, 3.6f, hookableLayer);
        RaycastHit2D hitPull = Physics2D.Raycast(origin, direction, 3.6f, pullableLayer);

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

            drawTongueHookPoint = hookPoint;

        }
        else if (!PresingClick && isHooked)
        {
            drawTongueHookPoint = null;
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

            drawTonguePullPoint = pullPoint;

        }
        else if (Input.GetKeyDown(KeyCode.E) && isPulling)
        {
            dj.enabled = false;
            dj.connectedBody = null;
            isPulling = false;
            anim.SetBool("TonguePull", false);

            drawTonguePullPoint = null;
        }

        if (isPulling && PresingClick)
        {
            dj.distance -= 2f * Time.deltaTime;
            dj.distance = Mathf.Clamp(dj.distance, 1.4f, 4.3f);
            
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
            dj.distance = Mathf.Clamp(dj.distance, 0.2f, 3.5f);
            
            

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
    
    void UpdateTongueVisual()
    {
        // Comprobamos si el sapo está enganchado o tirando de un objeto
        if ((isHooked || isPulling) && dj.connectedBody != null)
        {
            spawnRope.SetActive(true); // activamos el objeto visual de la lengua

            Vector3 end = Vector3.zero; // posición del extremo final de la lengua
            Vector3 start = transform.position; // posición de inicio (boca del sapo)

            // donde se engancha la lengua en hook y en pull
            if (isHooked)
            {
                end = drawTongueHookPoint.transform.position; 
            }
            else if (isPulling)
            {
                end = drawTonguePullPoint.transform.position;
            }

            // Calcular dirección
            Vector3 dir = (end - start).normalized;

            // Offsets para que la lengua no salga exactamente de la posición central del sapo ni del centro del objeto
            float offsetEnd = 0.5f;       
            float offsetStartPull = 0.5f; 
            float offsetStartHook = 0.2f; 

            // Aplicamos el offset segun si estamos en hook o en pull
            if(isHooked)
            {
                start += dir * offsetStartHook;
            }
            else if(isPulling)
            {
                start += dir * offsetStartPull;
            }

            end -= dir * offsetEnd; // desplazamos el extremo final para que no esté dentro del collider
            float dist = Vector3.Distance(start, end); // distancia entre inicio y final

            //Colocar y rotar el spawnRope
            spawnRope.transform.position = start; 
            spawnRope.transform.right = dir;

            // Referencias a las partes de la lengua
            Transform inicio = spawnRope.transform.GetChild(0); 
            Transform medio = spawnRope.transform.GetChild(1);  
            Transform final = spawnRope.transform.GetChild(2);  

            //Movida de ejes locales para que se ajuste bien
            inicio.localPosition = Vector3.zero; 

            // Colocamos el medio a la mitad de la distancia y lo escalamos en X según la distancia
            medio.localPosition = new Vector3(dist / 2f, 0, 0);
            medio.localScale = new Vector3(1, dist, 1); // escalado en Y según distancia

            // El final lo ponemos justo en el extremo de la lengua
            final.localPosition = new Vector3(dist, 0, 0);
        }
        else
        {
            spawnRope.SetActive(false); // si no hay hook ni pull, ocultamos la lengua
        }
    }
}
