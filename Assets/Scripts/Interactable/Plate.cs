using UnityEngine;
using UnityEngine.UI;

public class Plate : InteractableObjects
{
    private RecipeManager recipeManager;
    public override bool GetCanBeInteracted(int handID)
    {
        if(recipeManager == null) recipeManager = gameObject.GetComponent<RecipeManager>();
        GameObject holdingItem = playerController.GetHoldingItem(handID);
        if(holdingItem == null){return false;}
        IngredientManager ingredientManager = holdingItem.GetComponent<IngredientManager>();
        return recipeManager.CanAddThisIngrediant(ingredientManager);
    }

    public override void Interact(int handID)
    {
        recipeManager.AddIngrediantToPlate(playerController.GetHoldingItem(handID));
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        playerController.ReleaseItem(handID);
    }

    public override void SetInteractText(){
        interactionText = $"put the item on the plate";
    }

    public override void ChangeInteractionMenuText(InterractionCanvas interactionMenuManager){
        interactionMenuManager.SetInteractionText($"Place the ingredient from your:");
    }
}
