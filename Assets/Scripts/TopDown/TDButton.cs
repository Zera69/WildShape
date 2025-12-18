using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDButton : MonoBehaviour
{
    public TDBarrier[] barriers;
    public Sprite buttonOn;
    public Sprite buttonOff;
    public bool isOn = true;

    private SpriteRenderer spRen;

    // Start is called before the first frame update
    void Start()
    {
        spRen=GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {
        //Debug.Log("Button Activated");
        if(isOn)
        {
            isOn = false;
            spRen.sprite = buttonOff;
            for(int i = 0; i < barriers.Length; i++)
            {
                barriers[i].turnOff();
            }
        } else
        {
            isOn = true;
            spRen.sprite = buttonOn;
            for (int i = 0; i < barriers.Length; i++)
            {
                barriers[i].turnOn();
            }
        }
    }
}
