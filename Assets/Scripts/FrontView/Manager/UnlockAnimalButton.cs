using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockAnimalButton : MonoBehaviour
{
    public string animalName;
    public FVUnlockManager FVUnlockManager;
    private bool activated = false;

    public void Activate()
    {
        // Evitar múltiples activaciones
        if (!activated)
        {
            // Desbloquear el animal usando FVUnlockManager con el nombre del animal public
            activated = true;
            FVUnlockManager.UnlockAnimal(animalName);
        }
    }

}
