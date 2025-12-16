using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FVDoor : MonoBehaviour
{
    float distance = 4f; 
    float speed = 1f;
    // Start is called before the first frame update
    public IEnumerator MoveUp()
    {
        float moved = 0f;
        while (moved < distance)
        {
            float step = speed * Time.deltaTime;
            transform.Translate(Vector3.up * step);
            moved += step;
            yield return null;
        }
    }

    public void Action()
    {
        StartCoroutine(MoveUp());
    }

}
