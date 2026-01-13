using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public static MenuManager instance;
    public GameObject pauseMenu;
    public GameObject mainUI;
    public GameObject mainMenu;
    public bool isPaused = false;
    public bool isInCharactersUI = false;
    SceneLoadManager sceneLoadManager;

    public Image druid;
    public Image bear;
    public Image squirrel;
    public Image toad;

    private float time = 1f;

    private Color black = new Color(0, 0, 0, 1);
    private Color white = new Color(1, 1, 1, 1);


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
        mainUI.SetActive(false);
        //NewScene();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex!=0)
        {
            if(isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    IEnumerator showMain()
    {
        yield return new WaitForSeconds(time);
        mainUI.SetActive(false);
        mainMenu.SetActive(true);
    }

    IEnumerator hideMain()
    {
        mainMenu.SetActive(false);

        yield return new WaitForSeconds(time);
        
        mainUI.SetActive(true);
    }

    public void PlayGame()
    {
        sceneLoadManager = FindObjectOfType<SceneLoadManager>();
        sceneLoadManager.NextScene();
        StartCoroutine(hideMain());
        
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
    }

    public void ReturnToMainMenu()
    {
        sceneLoadManager = FindObjectOfType<SceneLoadManager>();
        ResumeGame();
        sceneLoadManager.ReturnToMainMenu();
        
        StartCoroutine(showMain());
    }


    public void UpdateCharacters()
    {
        //chequeo json y ver que imagenes de personaje se activan
        SaveData sd = SaveManager.instance.GetData();

        if (sd.unlockedCharacters.Contains("Druid"))
        {
            druid.color = white;
        }
        else
        {
            druid.color = black;
        }

        if (sd.unlockedCharacters.Contains("Bear"))
        {
            bear.color = white;
        }
        else
        {
            bear.color = black;
        }

        if (sd.unlockedCharacters.Contains("Squirrel"))
        {
            squirrel.color = white;
        }
        else
        {
            squirrel.color = black;
        }

        if (sd.unlockedCharacters.Contains("Toad"))
        {
            toad.color = white;
        }
        else
        {
            toad.color = black;
        }

    }
}
