using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndFade : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeStart()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
        anim.SetBool("end", false);
        anim.SetBool("start", true);
    }

    public void FadeEnd()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
        anim.SetBool("start", false);
        anim.SetBool("end", true);
    }
}
