using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDFrog : MonoBehaviour
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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            anim.SetBool("TongueOut", true);
        } else if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            anim.SetBool("TongueOut", false);
        }
    }
}
