using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDPlate : MonoBehaviour
{
    public TDBarrier[] barriers;
    public Sprite plateUp;
    public Sprite plateDown;
    public bool isDownBarrier = false;

    private SpriteRenderer spRen;

    // Start is called before the first frame update
    void Start()
    {
        spRen = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDownBarrier && collision.gameObject.CompareTag("Box" ))
        {
            isDownBarrier = true;
            spRen.sprite = plateDown;

            if(barriers.Length > 0 )
            {
                for (int i = 0; i < barriers.Length; i++)
                {
                    barriers[i].Active();
                    Debug.Log("turn off barrier");
                }
            }
            
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (isDownBarrier && collision.gameObject.CompareTag("Box" ))
        {
            isDownBarrier = false;
            spRen.sprite = plateUp;

            if(barriers.Length > 0 )
            {
                for (int i = 0; i < barriers.Length; i++)
                {
                    barriers[i].Active();
                }
            }
        }
    }
}
