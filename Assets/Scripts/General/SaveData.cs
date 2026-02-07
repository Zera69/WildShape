using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    //Clase que guardaremos en JSON con los datos del juego
    public List<string> unlockedCharacters = new List<string>{"Druid"};
    public List<string> completedLevels = new List<string>();
    public int CurrentCharacterIndex = 0;
    public int currentLevel;
    public bool saveDataExists = false;
    public List<string> stringsCartelesSapo = new List<string> { 
        "Puedes enagncharte en los bloques verdes clickando sobre ellos a cierta distancia, " +
        "mientras mantengas click seguiras enganchado y podras balancearte usando A Y D ," +
        " podras subir y bajar por tu lengua usando W y S, " +
        "una vez hayas cogido inercia balanceandote podras soltarte soltando el click"
        
        , "Cartel 2" };



}
