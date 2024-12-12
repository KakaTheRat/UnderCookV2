using UnityEngine;
using UnityEngine.UI;

public class Bin : InteractableObjects
{
    public override bool GetCanBeInteracted()
    {
        GameObject holdingItem = playerController.GetHoldingItem();
        if(holdingItem != null){
            return true;
        }
        else{return false;}
    }

    public override void Interact()
    {
        playerController.DestroyHoldingItem();
    }

    public override void SetInteractText(){
        GameObject playerHolding = playerController.GetHoldingItem();
        if(playerHolding != null){
            interactionText = $"throw the {playerHolding.GetComponent<IngredientManager>().GetIngredientName()}";
        }
    }

    public override void ChangeInteractionMenuText(InterractionCanvas interactionMenuManager){
        interactionMenuManager.SetInteractionText("Throw the item in:");
    }
    public override void ChangeButtonsAction(InterractionCanvas interactionMenuManager){
        foreach(Button button in interactionMenuManager.GetButtons()){
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener( () => Interact());
        }
    }
}
