using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public GameObject[] lista;
    private Vector2 pos;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("FV_Druida");
    }

    // Update is called once per frame
    void Update()
    {
        pos = player.transform.position;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Destroy(player);
            Instantiate(lista[0], pos, lista[0].transform.rotation);
            player = GameObject.Find("FV_Druida(Clone)");
        } 
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Destroy(player);
            Instantiate(lista[1], pos, lista[1].transform.rotation);
            player = GameObject.Find("FV_Ardilla(Clone)");
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            Destroy(player);
            Instantiate(lista[2], pos, lista[2].transform.rotation);
            player = GameObject.Find("FV_Sapo(Clone)");
        }
    }
}
