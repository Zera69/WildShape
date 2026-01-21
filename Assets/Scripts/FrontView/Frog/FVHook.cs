using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FVHook : MonoBehaviour
{

    private float forceInHook = 20f;
    private float impulseHook = 15f;

    public float inputHorizontal;
    public float inputVertical;

    private Rigidbody2D rb;

    public bool isHooked = false;
    private bool isPulling = false;
    private bool PresingClick;

    public DistanceJoint2D dj;

    public LayerMask interactionLayer;

    private GameObject hookPoint;
    private GameObject pullPoint;
    private GameObject buttonPoint;
    private GameObject drawTongueHookPoint;
    private GameObject drawTonguePullPoint;
    public GameObject spawnRope;
   

    private Camera cam;
    public FVSapo ScriptSapo;

    private Animator anim;


    private Vector2 origin;
    private Vector2 direction;

    private bool ThrowingTongue = false;

    private float maxYDifferencePull = 0.5f;
    private bool OverMaxYDifferencePull = false;
    private float yDifferencePull;

    private float maxYDiferenceButton = 0.5f;
    private bool OverMaxYDifferenceButton = false;
    float yDifferenceButton;

    //Tongue button
    private float tongueSpeed = 0.1f;         // Velocidad de extensión de la lengua
    private float maxInteractDistance = 4.5f;
    private Rigidbody2D rbPull;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Interact();
        HandleInput();
        DetectPresing();
        ChangeStatsInPullOrButton();
        ChangeMassHooked();
        ChechPullDistanceY();



    }

    void FixedUpdate()
    {
        MoveOnHook();
        UpdateTongueVisual();
    }

    
    void ChechPullDistanceY()
    {
        //Comprobamos continuamente si nos hemos pasado del maximo en Y al pullear
        if(isPulling && pullPoint != null)
        {
            //Comprobamos la diferencia en Y entre el sapo y el punto de pull
            yDifferencePull = Mathf.Abs(pullPoint.transform.position.y - transform.position.y);
            //Si la diferencia es mayor al maximo permitido soltamos el pull
            if(yDifferencePull > maxYDifferencePull)
            {
                ReleasePull();
            }
           
        }
    }

    //Soltamos el pull
    void ReleasePull()
    {
        dj.enabled = false;
        dj.connectedBody = null;
        isPulling = false;
        anim.SetBool("TonguePull", false);
        drawTonguePullPoint = null; 
    }

    void Interact()
    {
        Debug.DrawRay(transform.position, direction * 4.5f, Color.red);
        //Miramos donde esta la posicion del raton
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        //Miramos nuestra posicion y calculamos la direccion hacia el raton
        origin = transform.position;
        direction = (mouseWorldPos - transform.position).normalized;

        if(direction.x > 0)
        {
            anim.SetFloat("PullX", 1);
        } else if(direction.x < 0)
        {
            anim.SetFloat("PullX", -1);
        }

        //Raycasts de cada accion
        RaycastHit2D hitPull = new RaycastHit2D();
        RaycastHit2D hitHook = new RaycastHit2D();
        RaycastHit2D hitButton = new RaycastHit2D();

        //OverlapPoint desde el raton
        Collider2D mouseCollider = Physics2D.OverlapPoint(mouseWorldPos, interactionLayer);
        
        //Si no es null esque hay algo con lo que interactuar en el raton
        if (mouseCollider != null)
        {
            //Origen y target
            Vector2 origin = transform.position;
            Vector2 target = mouseCollider.transform.position;
            
            float dist = Vector2.Distance(origin, target);

            //Si estamos a la distancia permitida podemos interactuar
            if(dist <= maxInteractDistance)
            {
                Vector2 dir = (target - origin).normalized;

                //Ray cast generico
                RaycastHit2D hit = Physics2D.Raycast(origin, dir, dist, interactionLayer);

                if (hit && hit.collider == mouseCollider)
                {
                    //Depende el tag lo ponemos a una accion o otra
                    switch (hit.collider.tag)
                    {
                        case "Box":
                            hitPull = hit;
                            break;

                        case "Hookable":
                            hitHook = hit;
                            break;

                        case "Button":
                            hitButton = hit;
                            break;
                    }
                } 
            }
            
        }

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

        if (hitButton.collider != null)
        {
            buttonPoint = hitButton.collider.gameObject;
        }
        
        if(buttonPoint!= null)
        {
            //Comprobamos la diferencia en Y entre el sapo y el button
            yDifferenceButton = Mathf.Abs(buttonPoint.transform.position.y - transform.position.y);
        }

        //Si la diferencia es mayor al maximo permitido activamos la variable
        if (yDifferenceButton > maxYDiferenceButton)
        {
            OverMaxYDifferenceButton = true;
        }
        else
        {
            OverMaxYDifferenceButton = false;
        }
        //Si hacmeos click izquierdo y detectamos un boton entramos en el if
        if (Input.GetMouseButtonDown(0) && hitButton.collider != null && !OverMaxYDifferenceButton && hitButton.collider.tag == "Button")
        {
            //si no estamos ya tirando de la lengua inciamos animacion
            if(!ThrowingTongue)
            {
                FVButton buttonSapo = buttonPoint.GetComponent<FVButton>();
                StartCoroutine(TongueButtonRoutineStart(buttonPoint.transform.position,buttonSapo)); 
            }
            

        }

        

        //Si hacmeos click izquierdo, no estamos cogidos y detectamos donde cogernos entramos en el if
        if (PresingClick && !isHooked && hitHook.collider != null)
        {
            //activamos la conexion
            dj.enabled = true;
            dj.connectedBody = hitHook.collider.GetComponent<Rigidbody2D>();
            isHooked = true;
            anim.SetBool("TongueOut", true);

            drawTongueHookPoint = hookPoint;

        }
        //Si no esta presionando el click y estamos cogidos, soltamos el gancho
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

        if(pullPoint != null)
        {
            //Comprobamos la diferencia en Y entre el sapo y el punto de pull
            yDifferencePull = Mathf.Abs(pullPoint.transform.position.y - transform.position.y);
        }

        //Si la diferencia es mayor al maximo permitido activamos la variable
        if (yDifferencePull > maxYDifferencePull)
        {
            OverMaxYDifferencePull = true;
        }
        else
        {
            OverMaxYDifferencePull = false;
        }

        //Si hacemos click izquierda, no estamos pulleando, no estamos en el aire , detectamos donde coger y no es un button entramos en el if
        if (PresingClick && hitPull.collider != null && !isPulling && ScriptSapo.onFloor == true && !OverMaxYDifferencePull && hitPull.collider.tag != "Button")
        {
            rbPull = pullPoint.GetComponent<Rigidbody2D>();
            rbPull.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
            rbPull.bodyType = RigidbodyType2D.Dynamic;
            Debug.Log(hitPull.collider.tag);
            //activamos la conexion
            dj.enabled = true;
            dj.connectedBody = hitPull.collider.GetComponent<Rigidbody2D>   ();
            isPulling = true;
            anim.SetBool("TonguePull", true);

            drawTonguePullPoint = pullPoint;

        }
        //Si dejamos de presioanr click y estamos pulleando, soltamos el objeto
        else if (!PresingClick && isPulling)
        {
            rbPull.constraints |= RigidbodyConstraints2D.FreezePositionX;
            rbPull.bodyType = RigidbodyType2D.Kinematic;
            dj.enabled = false;
            dj.connectedBody = null;
            isPulling = false;
            anim.SetBool("TonguePull", false);

            drawTonguePullPoint = null;
        }

        //Si estamso pulleando y presionamos la E, acercamos el objeto
        if (isPulling && Input.GetKey(KeyCode.E))
        {
            dj.distance -= 2f * Time.deltaTime;
            dj.distance = Mathf.Clamp(dj.distance, 1.4f, 5f);
            
        }


    }

    public void DesactiveHookAndPull()
    {
        dj.enabled = false;
        dj.connectedBody = null;
        isPulling = false;
        isHooked = false;
    }



    void MoveOnHook()
    {
        //Si estamos cogidos al gancho
        if (isHooked)
        {
            // Añadir fuerza horizontal al player con las teclas A y D
            Vector2 force = new Vector2(inputHorizontal * forceInHook, 0);
            rb.AddForce(force);

            //Acercar y alejar el gancho con las teclas W y S
            dj.distance -= inputVertical * 2f * Time.deltaTime;
            //Limitar la distancia minima y maxima del gancho
            dj.distance = Mathf.Clamp(dj.distance, 1.2f, 4.1f);
            
            

        }
    }

    //Fuerza añadida al salir del gancho para simular inercia
    void ImpulseOnExitHook()
    {
        rb.AddForce(rb.velocity.normalized * impulseHook,ForceMode2D.Impulse);
    }

    //Cambiamos la masa y las propiedades del sapo al estar pulleando
    void ChangeStatsInPullOrButton()
    {
        if(ThrowingTongue)
        {
            ScriptSapo.velocidad = 0f;
            ScriptSapo.fuerzaSalto = 0f;
        }else if (isPulling)
        {
            //Si estamos pulleando, no podremos movernos y solo saltar un poco
            ScriptSapo.velocidad = 0f;
            ScriptSapo.fuerzaSalto = 4f;
            rb.mass = 10;
        }
        else 
        { 
            //Si no estamos pulleando, volvemos a la normalidad
            ScriptSapo.velocidad = 3f;
            ScriptSapo.fuerzaSalto = 15f;
            rb.mass = 5;

        }
    }

    //Cambiamos la masa del sapo al estar enganchado para no balancearse tanto
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

    //Detectamos la entrada del jugador
    void HandleInput()
    {
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");
    }

    //Detectamos si se esta presionando el click izquierdo
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
    
    //Actualizamos la visual de la lengua segun si estamos enganchados o pulleando
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
        else if(!ThrowingTongue)
        {
            spawnRope.SetActive(false); // si no hay hook ni pull, ocultamos la lengua
        }
    }

    IEnumerator TongueButtonRoutineStart(Vector3 hitPoint,FVButton buttonSapo)
    {
        ScriptSapo.velocidad = 0f; // Bloqueamos el movimiento del sapo mientras se extiende la lengua
        // Mostramos la animación de lanzar la lengua
        anim.SetBool("TonguePull", true);
        yield return new WaitForSeconds(0.05f);
        // Indicamos que estamos lanzando la lengua
        ThrowingTongue = true;
        // Mostramos la lengua
        spawnRope.SetActive(true);

        // Posiciones
        Vector3 start = transform.position;
        Vector3 end = hitPoint;

        // Dirección
        Vector3 dir = (end - start).normalized;

        // Offsets para que la lengua no salga exactamente de la posición central del sapo
        float offsetStart = 0.4f;
        start += dir * offsetStart;

        // Distancia total a recorrer
        float targetLength = Vector3.Distance(start, end);

        // Longitud actual de la lengua o Distancia recorrida
        float currentLength = 0f;

        // Colocación base
        spawnRope.transform.position = start;
        spawnRope.transform.right = dir;

        // Referencias a las partes de la lengua
        Transform inicio = spawnRope.transform.GetChild(0);
        Transform medio = spawnRope.transform.GetChild(1);
        Transform final = spawnRope.transform.GetChild(2);

        // Posición inicial del inicio de la lengua
        inicio.localPosition = Vector3.zero;

        // "Animacion" hasta que llegue a su objetivo
        while (currentLength < targetLength)
        {
            // Incrementamos la longitud actual
            currentLength += tongueSpeed;
            // Esperamos un frame
            yield return new WaitForSeconds(0.003f);
            // Aseguramos que no sobrepasa la longitud objetivo
            currentLength = Mathf.Min(currentLength, targetLength);

            // Actualizamos la posición y escala de las partes de la lengua
            medio.localPosition = new Vector3(currentLength / 2f, 0, 0);
            medio.localScale = new Vector3(1, currentLength, 1);
            final.localPosition = new Vector3(currentLength, 0, 0);
        }

        // Pequeña pausa tocando el botón
        yield return new WaitForSeconds(0.2f);
        if(!isPulling)
        {
            StartCoroutine(TongueButtonRoutineFinish(targetLength));
            buttonSapo.Activate(); 
        }
        
    }

    IEnumerator TongueButtonRoutineFinish(float startLenght)
    {
        // CurrentLength empieza en la longitud máxima
        float currentLength = startLenght;

        // Referencias a las partes de la lengua
        Transform inicio = spawnRope.transform.GetChild(0);
        Transform medio = spawnRope.transform.GetChild(1);
        Transform final = spawnRope.transform.GetChild(2);

        // Posición inicial del inicio de la lengua
        inicio.localPosition = Vector3.zero;

        // "Animacion" hasta que llegue a su objetivo
        while (currentLength > 0f)
        {
            // Deincrementamos la longitud actual
            currentLength -= tongueSpeed;
            // Esperamos un frame
            yield return new WaitForSeconds(0.003f);
            // Aseguramos que no sobrepasa la longitud objetivo
            currentLength = Mathf.Max(currentLength, 0f);

            // Actualizamos la posición y escala de las partes de la lengua
            medio.localPosition = new Vector3(currentLength / 2f, 0, 0);
            medio.localScale = new Vector3(1, currentLength, 1);
            final.localPosition = new Vector3(currentLength, 0, 0);
        }

        anim.SetBool("TonguePull", false);

        // Ocultamos lengua
        spawnRope.SetActive(false);
        ThrowingTongue = false;
        ScriptSapo.velocidad = 3f;
    }









    

}
