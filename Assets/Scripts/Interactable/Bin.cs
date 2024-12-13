using UnityEngine;
using UnityEngine.UI;

public class Bin : InteractableObjects
{
    public override bool GetCanBeInteracted(int handId)
    {
        GameObject holdingItem = playerController.GetHoldingItem(handId);
        if(holdingItem != null){
            return true;
        }
        else{return false;}
    }

    public override void Interact(int handId)
    {
        playerController.DestroyHoldingItem(handId);
    }

    public override void SetInteractText(){
        interactionText = $"throw the item from your:";
    }

    public override void ChangeInteractionMenuText(InterractionCanvas interactionMenuManager){
        interactionMenuManager.SetInteractionText("Throw the item in your:");
    }
}
