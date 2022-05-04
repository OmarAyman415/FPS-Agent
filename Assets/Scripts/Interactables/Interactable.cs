using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    //Message displayed tp the player when looking at an interactable
    public string promptMessage = "This default message";

    //this function will be called from our player
    public void BaseInteract()
    {
        Interact();
    }
    protected virtual void Interact() { }
}
