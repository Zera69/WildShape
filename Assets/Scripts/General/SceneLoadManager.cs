using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoadManager : MonoBehaviour
{
    public Image imagenDeFondo;
    public float duracion = 1f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeFromBlack());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
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

    IEnumerator LoadWithFadePreviousScene()
    {
        yield return FadeToBlack();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex - 1);
    }
    
    IEnumerator LoadWithFadeNextScene()
    {
        yield return FadeToBlack();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    IEnumerator LoadMainMenu()
    {
        yield return FadeToBlack();
        SceneManager.LoadScene(0);
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
    }

    public IEnumerator FadeToBlack()
    {
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
