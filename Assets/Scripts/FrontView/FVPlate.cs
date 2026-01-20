using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FVPlate : MonoBehaviour
{
    //private bool activated = false;
    public FVDoor[] doors;
    public Sprite plateUp;
    public Sprite plateDown;
    public bool isDown = false;
    private int count = 0;

    private SpriteRenderer spRen;

    // Start is called before the first frame update
    void Start()
    {
        spRen = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDown && count==0)
        {
            isDown = true;
            spRen.sprite = plateDown;

            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].turnOff();
            }
        }

        count++;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        count--;

        if (isDown && count==0)
        {
            isDown = false;
            spRen.sprite = plateUp;

            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].turnOn();
            }
        }
    }
}
