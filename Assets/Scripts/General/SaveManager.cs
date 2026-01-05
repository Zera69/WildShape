using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{

    //Instancia del SaveManager 
    public static SaveManager instance{ get; private set;}

    //Ruta donde se guardaran los datos
    private string savePath;

    //Datos actuales del juego
    private SaveData currentData;


    //Detectamos el F12 para resetear el juego en modo desarrollo
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.F12))
        {
            ResetGame();
        }
    }

    //Implementamos el singleton y cargamos los datos guardados
    private void Awake() 
    {
        //Implementacion del patron Singleton
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        //Definicion de la ruta de guardado
        savePath = Application.persistentDataPath + "/save.json";
        //Cargamos los datos guardados
        LoadGame();
    }
    
    //Obtenemos los datos actuales del juego
    public SaveData GetData()
    {
        return currentData;
    }
    
    //Cargamos los datos guardados desde el archivo
    public void LoadGame()
    {
        //Si el archivo existe, lo leemos y deserializamos los datos
        if(File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            currentData = JsonUtility.FromJson<SaveData>(json);
        }
        //Si no existe, creamos un nuevo archivo con datos por defecto
        else
        {
            currentData = new SaveData();
            SaveGame();
        }
    }

    //Guardamos los datos actuales del juego en el archivo
    public void SaveGame()
    {
        string json = JsonUtility.ToJson(currentData, true);
        File.WriteAllText(savePath, json);
    }

    //Reseteamos el juego a los datos por defecto  
    public void ResetGame()
    {
        currentData = new SaveData();
        SaveGame();
        Debug.Log("Juego reseteado");
    }
}
