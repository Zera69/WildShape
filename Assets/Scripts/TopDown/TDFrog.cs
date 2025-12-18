using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDFrog : MonoBehaviour
{
    private Animator anim;
    private Camera cam;
    private Vector2 origin;
    private Vector2 direction;
    public LayerMask buttonLayer;     
    private float tongueSpeed = 0.1f;
    private bool ThrowingTongue = false;
    private GameObject buttonPoint;
    public GameObject spawnRope;
    

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            anim.SetBool("TongueOut", true);
        } else if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            anim.SetBool("TongueOut", false);
        }
        DetectButtonClick();
    }

    void DetectButtonClick()
    {
        //Miramos donde esta la posicion del raton
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        //Miramos nuestra posicion y calculamos la direccion hacia el raton
        origin = transform.position;
        direction = (mouseWorldPos - transform.position).normalized;

        //Detectamos la direccion para la animacion
        if(direction.x > 0)
        {
            anim.SetFloat("PullX", 1);
        } else if(direction.x < 0)
        {
            anim.SetFloat("PullX", -1);
        }

        //Lanzmaos un raycast desde nosotros hacia el raton
        RaycastHit2D hitButton = Physics2D.Raycast(origin, direction, 6f, buttonLayer);

        //Lo dibujamos en pantalla
        Debug.DrawRay(origin, direction * 6f, Color.blue);

        //Detectamos si hay algo que Buttonear a nuestro alcance
        if (hitButton.collider != null)
        {
            buttonPoint = hitButton.collider.gameObject;
        }

        //Si hacmeos click izquierdo y detectamos un boton entramos en el if
        if (Input.GetMouseButtonDown(0) && hitButton.collider != null)
        {
            //si no estamos ya tirando de la lengua inciamos animacion
            if(!ThrowingTongue)
            {
                TDButton buttonSapo = buttonPoint.GetComponent<TDButton>();
                StartCoroutine(TongueButtonRoutineStart(buttonPoint.transform.position,buttonSapo)); 
            }
            

        }
    } 

    IEnumerator TongueButtonRoutineStart(Vector3 hitPoint,TDButton buttonSapo)
    {
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
        StartCoroutine(TongueButtonRoutineFinish(targetLength));
        buttonSapo.Activate(); 
        
        
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
    }
}
