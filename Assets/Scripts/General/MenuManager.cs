using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public static MenuManager instance;
    public GameObject pauseMenu;
    public GameObject charactersUI;
    public CharactersUI charactersUIScript;
    public bool isPaused = false;
    public bool isInCharactersUI = false;
    // Start is called before the first frame update
    void Start()
    {
        //Singleton del MenuManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                if(isInCharactersUI)
                {
                    charactersUIScript.BackToMenu();
                    isInCharactersUI = false;
                }else
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game (aunque en editor no se vea)");
    }

    public void GoToCharactersUI()
    {
        isInCharactersUI = true;
        pauseMenu.SetActive(false);
        charactersUI.SetActive(true);
    }
}
