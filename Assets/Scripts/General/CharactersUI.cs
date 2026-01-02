using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharactersUI : MonoBehaviour
{
    private SaveData data;
    public GameObject squirrelBG;
    public GameObject frogBG;
    public GameObject bearBG;
    private Color unlockedColor = new Color(0.31f, 1f, 0.902f, 1f);
    private Image imgSquirrel;
    private Image imgFrog;
    private Image imgBear;

    public GameObject pauseMenu;
    public MenuManager menuManager;
    // Start is called before the first frame update
    void Start()
    {
        imgSquirrel = squirrelBG.GetComponent<Image>();
        imgFrog = frogBG.GetComponent<Image>();
        imgBear = bearBG.GetComponent<Image>();
        data = SaveManager.instance.GetData();
    }

    // Update is called once per frame
    void Update()
    {
        updateCharactersUI();
    }

    private void updateCharactersUI()
    {
        //Comprobar si el personaje esta desbloqueado y activar su fondo en la UI
        if(data.unlockedCharacters.Contains("Squirrel"))
        {
            imgSquirrel.color = unlockedColor;
        }
        if(data.unlockedCharacters.Contains("Frog"))
        {
            imgFrog.color = unlockedColor;
        }
        if(data.unlockedCharacters.Contains("Bear"))
        {
            imgBear.color = unlockedColor;
        }
    }

    public void BackToMenu()
    {
        menuManager.isInCharactersUI = false;
        pauseMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
