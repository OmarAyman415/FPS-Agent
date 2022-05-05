using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // add or remove an interactionEvent component to this gameobject 
    public bool useEvents;

    //Message displayed tp the player when looking at an interactable
    [SerializeField]
    public string promptMessage = "This default message";

    public virtual string OnLook(){
        return promptMessage;
    }
    //this function will be called from our player
    public void BaseInteract()
    {
        if(useEvents)
            GetComponent<InteractionEvent>().onInteract.Invoke();
        Interact();
    }
    protected virtual void Interact() { }
}
