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



}
