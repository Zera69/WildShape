using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CartelesManager : MonoBehaviour
{
  
    private SaveData data;
    public GameObject UI;
    private bool open = false;
    private bool inSing;
    // Start is called before the first frame update
    void Start()
    {
        data = SaveManager.instance.GetData();
    }

    // Update is called once per frame
    void Update()
    {
        DetectE();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            inSing = false;
            open = false;
            if(UI != null)
            {
                UI.SetActive(false);
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            inSing = true;
        }
    }

    private void DetectE()
    {
        if(Input.GetKeyDown(KeyCode.E) && !open && inSing)
        {
            open = true;
            UI.SetActive(true);
            //string txt = data.stringsCartelesSapo[index];
            //textoUI.text = txt;
        }
        else if (Input.GetKeyDown(KeyCode.E) && open)
        {
            open = false;
            UI.SetActive(false);
        }
    }
}
