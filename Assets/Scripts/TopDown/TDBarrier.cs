using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDBarrier : MonoBehaviour
{
    private Collider2D collider;
    private Animator anim;
    public bool isActive = true;
    public bool startsActive = true;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        if(!startsActive)
        {
            turnOff();
            isActive = false;
        }else
        {
            turnOn();
            isActive = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Active()
    {
        if(isActive)
        {
            turnOff();
            isActive = false;
        }
        else
        {
            turnOn();
            isActive = true;
        }
    }

    public void turnOn()
    {
        AudioManager.Instance.PlaySFX("barrier");
        anim.SetBool("Active", true);
        collider.enabled = true;
    }

    public void turnOff()
    {
        AudioManager.Instance.PlaySFX("barrier");
        anim.SetBool("Active", false);
        collider.enabled = false;
    }

}
