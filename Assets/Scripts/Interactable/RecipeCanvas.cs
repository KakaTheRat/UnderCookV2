using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class RecipeCanvas : InteractableObjects
{
    [SerializeField] private TMP_Text RecipeName;
    [SerializeField] private GameObject IngredientInfo;
    [SerializeField] private GameObject panel;
    private int actualRecipe = -1;
    private List<GameObject> ingredientsTexts = new List<GameObject>();
    public override void Interact(int hand)
    {
        actualRecipe ++;
        JsonManager jsonManager = FindObjectOfType<JsonManager>();
        if(actualRecipe > jsonManager.GetFoodInfo().recipes.Count - 1) actualRecipe = 0;
        Recipe recette = jsonManager.GetFoodInfo().recipes[actualRecipe];
        RecipeName.text = recette.name;
        foreach(GameObject ingredientText in ingredientsTexts){
            Destroy(ingredientText);
        }
        ingredientsTexts.Clear();
        foreach(IngredientInRecipe ingredient in recette.ingredients){
            GameObject newIngredientInfo = Instantiate(IngredientInfo, panel.transform);
            for(int i = 0; i < newIngredientInfo.transform.childCount; i++){
                TMP_Text tmp_text = newIngredientInfo.transform.GetChild(i).GetComponent<TMP_Text>();
                if(tmp_text.text == "Ingredient"){
                    tmp_text.text = ingredient.name;
                } else if(tmp_text.text == "Cut"){
                    string cross = "";
                    if(ingredient.cut){cross = "X";}
                    tmp_text.text = cross;
                } else{
                    string cross = "";
                    if(ingredient.cook){cross = "X";}
                    tmp_text.text = cross;
                }
            }
            ingredientsTexts.Add(newIngredientInfo);
        }
    }

    public override void SetInteractText(){
        interactionText = "Next Recipe" ;
    }

    public override void UpdateAndShowInteractionMenu(InterractionCanvas interactionMenuManager){}


}
