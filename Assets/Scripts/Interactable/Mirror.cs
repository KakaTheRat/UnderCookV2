using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : InteractableObjects
{
    public override void Interact()
    {
        playerController.Emote();
    }
    public override void SetInteractText(){
        interactionText = "Emote" ;
    }
}
