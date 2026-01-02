using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    //Clase que guardaremos en JSON con los datos del juego

    public List<string> unlockedLevels = new List<string>{"Level1"};
    public List<string> unlockedCharacters = new List<string>{"Druid"};
    
    
}
