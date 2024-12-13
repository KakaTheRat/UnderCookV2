using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : InteractableObjects
{
    public override void Interact(int hand)
    {
        playerController.Emote();
    }
    public override void SetInteractText(){
        interactionText = "Emote" ;
    }

    public override void UpdateAndShowInteractionMenu(InterractionCanvas interactionMenuManager){}
}
