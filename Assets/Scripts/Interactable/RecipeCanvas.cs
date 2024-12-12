using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeCanvas : InteractableObjects
{
    private int actualRecipe = 0;
    public override void Interact()
    {
        actualRecipe ++;
        JsonManager jsonManager = FindObjectOfType<JsonManager>();
        if(actualRecipe > jsonManager.GetFoodInfo().recipes.Count - 1) actualRecipe = 0;
        UIManager uIManager = FindObjectOfType<UIManager>();
        uIManager.SetRecipe(jsonManager.GetFoodInfo().recipes[actualRecipe]);
    }

    public override void SetInteractText(){
        interactionText = "Next Recipe" ;
    }

}
