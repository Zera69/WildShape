using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformWheel : MonoBehaviour
{
    public GameObject wheelCanvas;
    private Vector2 centerMousePos;
    private Vector2 currentDir;
    public RectTransform wheelPanel;
    public WheelDirection currentDirection = WheelDirection.None;
    private float deadZone = 20f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public enum WheelDirection
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            OpenWheel();
        }else if(Input.GetKeyUp(KeyCode.Tab))
        {
            CloseWheel();
        }

        if(Input.GetKey(KeyCode.Tab))
        {
            DetectDirection();
        }
    }

    void OpenWheel()
    {
        centerMousePos = Input.mousePosition;
        wheelCanvas.SetActive(true);
        wheelPanel.position = centerMousePos;
        Time.timeScale = 0.3f;
    }

    void CloseWheel()
    {
        wheelCanvas.SetActive(false);
        Time.timeScale = 1f;
        Debug.Log("Direction: " + currentDirection);
    }

    void DetectDirection()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 delta = mousePos - centerMousePos;

        //si apenas ha movido el raton, seleeccionar none
        if(delta.magnitude < deadZone)
        {
            currentDirection = WheelDirection.None;
        }
        //Si ha movido mas en x que en y, seleccionar izquierda o derecha dependiendo si es positivo o negativo el eje X
        else if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            currentDirection = delta.x > 0 ? WheelDirection.Right : WheelDirection.Left;
        }
        //Si ha movido mas en y que en x, seleccionar arriba o abajo dependiendo si es positivo o negativo el eje Y
        else
        {
            currentDirection = delta.y > 0 ? WheelDirection.Up : WheelDirection.Down;
        }
    }
}
