using UnityEngine;
using UnityEngine.UI;

public class Plate : InteractableObjects
{
    private RecipeManager recipeManager;
    public override bool GetCanBeInteracted()
    {
        if(recipeManager == null) recipeManager = gameObject.GetComponent<RecipeManager>();
        GameObject holdingItem = playerController.GetHoldingItem();
        if(holdingItem == null){return false;}
        IngredientManager ingredientManager = holdingItem.GetComponent<IngredientManager>();
        return recipeManager.CanAddThisIngrediant(ingredientManager);
    }

    public override void Interact()
    {
        recipeManager.AddIngrediantToPlate(playerController.GetHoldingItem());
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        playerController.ReleaseItem();
    }

    public override void SetInteractText(){
        GameObject playerHolding = playerController.GetHoldingItem();
        if(playerHolding != null){
            interactionText = $"put the {playerHolding.GetComponent<IngredientManager>().GetIngredientName()} on the plate";
        }
    }

    public override void ChangeInteractionMenuText(InterractionCanvas interactionMenuManager){
        interactionMenuManager.SetInteractionText($"Place the ingredient from your:");
    }
    public override void ChangeButtonsAction(InterractionCanvas interactionMenuManager){
        foreach(Button button in interactionMenuManager.GetButtons()){
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener( () => Interact());
        }
    }
}
