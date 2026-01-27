using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FVDoor : MonoBehaviour
{
    private Collider2D collider;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

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
