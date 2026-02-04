using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public static MenuManager instance;
    public GameObject pauseMenu;
    public GameObject mainUI;
    public GameObject mainMenu;
    public GameObject endScreen;

    public EndFade endBG;
    public GameObject endTxt;
    private float duracion = 2f;

    public GameObject audioMain;
    public GameObject pauseMenuGeneral;
    public GameObject pauseMenuAudio;
    public Slider musicSlider;
    public Slider sfxSlider;

    public GameObject continueBttn;
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
        checkDataExists();
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
        endScreen.SetActive(false);
        mainMenu.SetActive(true);
        checkDataExists();
    }

    IEnumerator hideMain()
    {
        mainMenu.SetActive(false);

        yield return new WaitForSeconds(time);
        
        mainUI.SetActive(true);
    }

    public void checkDataExists()
    {
        SaveData sd = SaveManager.instance.GetData();

        if (sd.saveDataExists)
        {
            continueBttn.SetActive(true);
        }
        else
        {
            continueBttn.SetActive(false);
        }
    }

    public void PlayGame()
    {
        AudioManager.Instance.PlaySFX("click");
        SaveManager.instance.ResetGame();
        sceneLoadManager = FindObjectOfType<SceneLoadManager>();
        sceneLoadManager.NextScene();
        StartCoroutine(hideMain());
        SaveData sd = SaveManager.instance.GetData();
        sd.saveDataExists = true;
        SaveManager.instance.SaveGame();
        AudioManager.Instance.PlayMusic("game");
    }

    public void MainMenuAudio()
    {
        AudioManager.Instance.PlaySFX("click");
        audioMain.SetActive(true);
    }

    public void MainMenuAudioBack()
    {
        AudioManager.Instance.PlaySFX("click");
        audioMain.SetActive(false);
    }

    public void MenuAudio()
    {
        AudioManager.Instance.PlaySFX("click");
        pauseMenuGeneral.SetActive(false);
        pauseMenuAudio.SetActive(true);
    }

    public void MenuAudioBack()
    {
        AudioManager.Instance.PlaySFX("click");
        pauseMenuAudio.SetActive(false);
        pauseMenuGeneral.SetActive(true);
    }

    public void ContinueGame()
    {
        AudioManager.Instance.PlaySFX("click");
        SaveData sd = SaveManager.instance.GetData();
        int scene = sd.currentLevel;
        sceneLoadManager = FindObjectOfType<SceneLoadManager>();
        sceneLoadManager.LoadScene(scene);
        StartCoroutine(hideMain());
        AudioManager.Instance.PlayMusic("game");
    }

    public void PauseGame()
    {
        AudioManager.Instance.PlaySFX("click");
        pauseMenuAudio.SetActive(false);
        pauseMenuGeneral.SetActive(true);
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        AudioManager.Instance.PlayMusic("pause");
    }

    public void ResumeGame()
    {
        AudioManager.Instance.PlaySFX("click");
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        AudioManager.Instance.PlayMusic("game");
    }

    public void QuitGame()
    {
        AudioManager.Instance.PlaySFX("click");
        Application.Quit();
    }

    public void ReturnToMainMenu()
    {
        sceneLoadManager = FindObjectOfType<SceneLoadManager>();
        ResumeGame();
        sceneLoadManager.ReturnToMainMenu();
        
        StartCoroutine(showMain());

        AudioManager.Instance.PlayMusic("main");
    }

    public void triggerEnd()
    {
        //pausar juego, pantalla fin, espera, main menú (borrar datos?)
        
        StartCoroutine(FadeToEnd());
    }

    public IEnumerator FadeToEnd()
    {
        endTxt.SetActive(false);
        endBG.FadeEnd();
        endScreen.SetActive(true);
        endBG.FadeStart();
        /*float tiempo = 0f;
        Color color = endBG.color;

        while (tiempo < duracion)
        {
            Debug.Log(tiempo);
            tiempo += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, tiempo / duracion);
            color.a = alpha;
            endBG.color = color;
            yield return null;
        }
        Debug.Log("3");
        color.a = 1f;
        endBG.color = color;*/
        yield return new WaitForSeconds(duracion);
        endTxt.SetActive(true);

        yield return new WaitForSeconds(duracion);
        
        ReturnToMainMenu();
        SaveManager.instance.ResetGame();
        
    }


    //-- AUDIO UI --//
    public void ToggleMusic()
    {
        AudioManager.Instance.PlaySFX("click");
        AudioManager.Instance.ToggleMusic();
    }

    public void ToggleSFX()
    {
        AudioManager.Instance.PlaySFX("click");
        AudioManager.Instance.ToggleSFX();
    }

    public void MusicVolume()
    {
        AudioManager.Instance.MusicVolume(musicSlider.value);
    }

    public void SFXVolume()
    {
        AudioManager.Instance.SFXVolume(sfxSlider.value);
    }

    //-- CHARACTER UI --//
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
