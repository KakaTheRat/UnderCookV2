using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[System.Serializable]
public class newIngredientInfo{

    [SerializeField] GameObject ingredientPreset;
    public TMP_InputField ingedientNameInputField {get;set;}
    public Toggle cut {get;set;}
    public Toggle cook {get;set;}

    public void Init(UnityEngine.Events.UnityAction<string> call){
        ingedientNameInputField = ingredientPreset.GetComponentInChildren<TMP_InputField>();
        ingedientNameInputField.onValueChanged.AddListener(call);
        foreach(Toggle toggle in ingredientPreset.GetComponentsInChildren<Toggle>()){
            if(toggle.gameObject.name.Contains("Cut")) cut = toggle;
            else if(toggle.gameObject.name.Contains("Cook")) cook = toggle;
        }
    }
}

public class NewRecipeCanvas : MonoBehaviour
{
    [SerializeField] RecipeCanvas recipeCanvas;
    [SerializeField] Button saveButon;
    [SerializeField] TMP_InputField recipeNameInputField;
    [SerializeField] List<newIngredientInfo> newIngredientInfos;

    void Awake(){
        recipeNameInputField.onValueChanged.AddListener(value => UpdateSaveButton());
        foreach(newIngredientInfo ingredientInfos in newIngredientInfos){
            ingredientInfos.Init(value => UpdateSaveButton());
        }
        saveButon.interactable = false;
    }

    public void Show(){
        recipeCanvas.RayCastOff();
        gameObject.SetActive(true);
    }

    public void Hide(){
        Reset();
        recipeCanvas.RayCastOn();
        gameObject.SetActive(false);
    }

    private bool CanSaveRecipe(){
        if(string.IsNullOrEmpty(recipeNameInputField.text))return false;
        bool canSave = false;
        foreach(newIngredientInfo ingredientInfo in newIngredientInfos){
            if(!string.IsNullOrEmpty(ingredientInfo.ingedientNameInputField.text)) canSave = true;
        }
        return canSave;
    }

    private void UpdateSaveButton(){
        saveButon.interactable = CanSaveRecipe();
    }

    private void Reset(){
        saveButon.interactable = false;
        recipeNameInputField.text = null;
        foreach(newIngredientInfo ingredientInfo in newIngredientInfos){
           ingredientInfo.ingedientNameInputField.text = null;
           ingredientInfo.cut.isOn = false;
           ingredientInfo.cook.isOn = false;
        }
    }

    public void Save(){
        Recipe recipe = new();
        recipe.name = recipeNameInputField.text;
        recipe.ingredients = new();
        foreach(newIngredientInfo ingredientInfo in newIngredientInfos){
            if(!string.IsNullOrEmpty(ingredientInfo.ingedientNameInputField.text)){
                IngredientInRecipe ingredient = new();
                ingredient.name = ingredientInfo.ingedientNameInputField.text;
                ingredient.cut = ingredientInfo.cut.isOn;
                ingredient.cook = ingredientInfo.cook.isOn;
                recipe.ingredients.Add(ingredient);
            }
        }
        recipeCanvas.AddNewRecipe(recipe);
        Reset();
    }
}
