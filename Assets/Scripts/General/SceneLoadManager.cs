using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoadManager : MonoBehaviour
{
    public Image imagenDeFondo;
    public float duracion = 1f;

    private MenuManager menuManager;

    public GameObject fade;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeFromBlack());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnToMainMenu()
    {
        StartCoroutine(LoadMainMenu());
    }

    public void NextScene()
    {
        StartCoroutine(LoadWithFadeNextScene());
    }

    public void PreviousScene()
    {
        StartCoroutine(LoadWithFadePreviousScene());
    }

    public void LoadScene(int s)
    {
        StartCoroutine(LoadWithFadeScene(s));
    }

    private void saveCurrentScene(int s)
    {
        if (s != 0)
        {
            SaveData sd = SaveManager.instance.GetData();
            sd.currentLevel = s;
            SaveManager.instance.SaveGame();
        }
    }

    IEnumerator LoadWithFadePreviousScene()
    {
        yield return FadeToBlack();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        saveCurrentScene(currentSceneIndex - 1);
        SceneManager.LoadScene(currentSceneIndex - 1);

        menuManager = FindObjectOfType<MenuManager>();
        menuManager.UpdateCharacters();
    }

    IEnumerator LoadWithFadeNextScene()
    {
        yield return FadeToBlack();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        saveCurrentScene(currentSceneIndex + 1);
        SceneManager.LoadScene(currentSceneIndex + 1);

        menuManager = FindObjectOfType<MenuManager>();
        menuManager.UpdateCharacters();
    }

    IEnumerator LoadWithFadeScene(int s)
    {
        yield return FadeToBlack();
        saveCurrentScene(s);
        SceneManager.LoadScene(s);

        menuManager=FindObjectOfType<MenuManager>();
        menuManager.UpdateCharacters();
    }

    IEnumerator LoadMainMenu()
    {
        yield return FadeToBlack();
        SceneManager.LoadScene(0);

        menuManager = FindObjectOfType<MenuManager>();
        menuManager.UpdateCharacters();
    }

    IEnumerator FadeFromBlack()
    {
        
        float tiempo = 0f;
        Color color = imagenDeFondo.color;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, tiempo / duracion);
            color.a = alpha;
            imagenDeFondo.color = color;
            yield return null;
        }

        color.a = 0f;
        imagenDeFondo.color = color;
        fade.SetActive(false);
    }

    public IEnumerator FadeToBlack()
    {
        fade.SetActive(true);
        float tiempo = 0f;
        Color color = imagenDeFondo.color;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, tiempo / duracion);
            color.a = alpha;
            imagenDeFondo.color = color;
            yield return null;
        }

        color.a = 1f;
        imagenDeFondo.color = color;
    }

}
