using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FVNextLevel : MonoBehaviour
{
    public SceneLoadManager sceneLoadManager;
    private SaveData data;
    public CharacterManager characterManager;


    private void Start()
    {
      data = SaveManager.instance.GetData();   
    }

    //Al tocar el limite
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            //Añadir el nivel completado a la lista
            string currentLevelName = SceneManager.GetActiveScene().name;
            int currentCharacterIndex = characterManager.GetCurrentCharacterIndex();
            data.CurrentCharacterIndex = currentCharacterIndex;
            if(!data.completedLevels.Contains(currentLevelName))
            {
                data.completedLevels.Add(currentLevelName);
            }
            //Cargar siguiente nivel
            Debug.Log("Cargando siguiente nivel...");
            sceneLoadManager.NextScene();
        }
       
    }
}
