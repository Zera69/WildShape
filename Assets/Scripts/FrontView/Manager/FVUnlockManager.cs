using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class FVUnlockManager : MonoBehaviour
{
    private SaveData data;


    //Cogemos la referencia a los datos guardados
    void Start()
    {
        data = SaveManager.instance.GetData();
    }

    //Metodo para desbloquear un animal
    public void UnlockAnimal(string animalName)
    {
        //comprobar si el animal ya esta en la lista de desbloqueados
        if (!data.unlockedCharacters.Contains(animalName))
        {
            //Si no esta, lo añadimos y guardamos los datos
            data.unlockedCharacters.Add(animalName);
            SaveManager.instance.SaveGame();
            Debug.Log("Animal desbloqueado: " + animalName);
        }
        else
        {
            //Si esta el animal, no hacemos nada
            Debug.Log("El animal ya estaba desbloqueado: " + animalName);
        }
    }



    //Metodo para comprobar si un animal esta desbloqueado  
    public bool IsAnimalUnlocked(string animalName)
    {
        return data.unlockedCharacters.Contains(animalName);
    }

   
}
