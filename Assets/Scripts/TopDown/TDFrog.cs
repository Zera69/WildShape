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
    private TDCharacterMovement characterMovement;

    // Ajustes para grid y top-down
    private float maxInteractDistance = 4f; // más pequeño que en front view
    private float maxYDiferenceButton = 0.5f; // menor diferencia de Y permitida
    private float yDifferenceButton = 0f;
    private bool OverMaxYDifferenceButton = false;

    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        characterMovement = GetComponent<TDCharacterMovement>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
       DetectButtonClick();
    }

    void DetectButtonClick()
    {
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        origin = transform.position;
        direction = (mouseWorldPos - transform.position).normalized;

        // Animación según dirección
        if(anim != null)
        {
            //Detectamos la direccion para la animacion 
            if(direction.x > 0) 
            { 
                //Mirar hacia la derecha
            } else if(direction.x < 0) 
            { 
                //Mirar hacia la izquierda
            }
        }

        // Detectamos botón con OverlapPoint + Raycast
        Collider2D mouseCollider = Physics2D.OverlapPoint(mouseWorldPos, buttonLayer);
        if(mouseCollider != null)
        {
            float dist = Vector2.Distance(origin, mouseCollider.transform.position);
            if(dist <= maxInteractDistance)
            {
                RaycastHit2D hitButton = Physics2D.Raycast(origin, ((Vector2)mouseCollider.transform.position - origin).normalized, dist, buttonLayer);

                if(hitButton.collider != null && hitButton.collider == mouseCollider)
                {
                    buttonPoint = hitButton.collider.gameObject;
                }
            }
        }else
        {
            buttonPoint = null;
        }

        if(buttonPoint != null)
        {
            yDifferenceButton = Mathf.Abs(buttonPoint.transform.position.y - transform.position.y);
            OverMaxYDifferenceButton = yDifferenceButton > maxYDiferenceButton;
        }

        if(Input.GetMouseButtonDown(0) && !characterMovement.IsMoving)
        {
            if(buttonPoint != null && !OverMaxYDifferenceButton)
            {
                if(!ThrowingTongue)
                {
                    anim.SetBool("TongueOut", true);
                    TDButton buttonSapo = buttonPoint.GetComponent<TDButton>();
                    if(buttonSapo != null)
                    {
                        StartCoroutine(TongueButtonRoutineStart(buttonPoint.transform.position, buttonSapo));
                    }   
                }
            }
        }
    }

    // El resto de los coroutines los dejamos igual
    IEnumerator TongueButtonRoutineStart(Vector3 hitPoint, TDButton buttonSapo)
    {
        yield return new WaitForSeconds(0.05f);
        ThrowingTongue = true;
        spawnRope.SetActive(true);

        Vector3 start = transform.position;
        Vector3 end = hitPoint;
        Vector3 dir = (end - start).normalized;

        float offsetStart = 0.4f;
        start += dir * offsetStart;

        float targetLength = Vector3.Distance(start, end);
        float currentLength = 0f;

        spawnRope.transform.position = start;
        spawnRope.transform.right = dir;

        Transform inicio = spawnRope.transform.GetChild(0);
        Transform medio = spawnRope.transform.GetChild(1);
        Transform final = spawnRope.transform.GetChild(2);

        inicio.localPosition = Vector3.zero;

        while(currentLength < targetLength)
        {
            currentLength += tongueSpeed;
            yield return new WaitForSeconds(0.003f);
            currentLength = Mathf.Min(currentLength, targetLength);

            medio.localPosition = new Vector3(currentLength / 2f, 0, 0);
            medio.localScale = new Vector3(1, currentLength, 1);
            final.localPosition = new Vector3(currentLength, 0, 0);
        }

        yield return new WaitForSeconds(0.2f);
        StartCoroutine(TongueButtonRoutineFinish(targetLength));
        buttonSapo.Activate();
        anim.SetBool("TongueOut", false);
    }

    IEnumerator TongueButtonRoutineFinish(float startLength)
    {
        float currentLength = startLength;

        Transform inicio = spawnRope.transform.GetChild(0);
        Transform medio = spawnRope.transform.GetChild(1);
        Transform final = spawnRope.transform.GetChild(2);

        inicio.localPosition = Vector3.zero;

        while(currentLength > 0f)
        {
            currentLength -= tongueSpeed;
            yield return new WaitForSeconds(0.003f);
            currentLength = Mathf.Max(currentLength, 0f);

            medio.localPosition = new Vector3(currentLength / 2f, 0, 0);
            medio.localScale = new Vector3(1, currentLength, 1);
            final.localPosition = new Vector3(currentLength, 0, 0);
        }

        spawnRope.SetActive(false);
        ThrowingTongue = false;
    }
}
