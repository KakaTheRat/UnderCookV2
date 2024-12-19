using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class RecipeCanvas : MonoBehaviour
{
    [SerializeField] private TMP_Text RecipeName;
    [SerializeField] private GameObject IngredientInfo;
    [SerializeField] private GameObject panel;
    [SerializeField] GameObject closeButton;
    [SerializeField] GameObject newRecipeButton;
    [SerializeField] NewRecipeCanvas newRecipeCanvas;
    private int actualRecipe = 0;
    private List<GameObject> ingredientsTexts = new List<GameObject>();
    GraphicRaycaster graphicRaycaster;

    RectTransform rectTransform;
    Vector3 defaultPosition;
    Quaternion defaultRotation;
    Vector3 defaultScale;
    Vector2 defaultSize;
    Canvas canvas;
    PlayerInput playerInput;
    List<Recipe> allRecipes;

    void Awake(){
        graphicRaycaster = GetComponent<GraphicRaycaster>();
        rectTransform = GetComponent<RectTransform>();
        defaultPosition = rectTransform.position;
        defaultRotation = rectTransform.rotation;
        defaultScale = rectTransform.localScale;
        defaultSize = rectTransform.rect.size;
        canvas =  GetComponent<Canvas>();
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        RayCastOff();
    }

    public void OnMouseUnlock(InputAction.CallbackContext context){
        if(context.started){
            RayCastOn();
        }
        if(context.canceled){
            RayCastOff();
        }
    }

    public void RayCastOn(){
        graphicRaycaster.enabled = true;
    }
    public void RayCastOff(){
        graphicRaycaster.enabled = false;
    }

    public void NextRecipe(){
        actualRecipe++;
        MaybeUpdateCanvas();
    }
    public void PreviousRecipe(){
        actualRecipe--;
        MaybeUpdateCanvas();
    }

    private void MaybeUpdateCanvas(){
        int lastactualRecipe = actualRecipe;
        actualRecipe = Math.Clamp(actualRecipe, 0, allRecipes.Count - 1);
        if(actualRecipe != lastactualRecipe)return;
        UpdateCanvas();
    }

    public void InitRecipes(List<Recipe> recipes){
        allRecipes = recipes;
        UpdateCanvas();
    }

    private void UpdateCanvas(){
        Recipe recette = allRecipes[actualRecipe];
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

    public void FullScreen(){
        playerInput.enabled = false;
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        RayCastOn();
        closeButton.SetActive(true);
        newRecipeButton.SetActive(true);
    }

    public void CloseFullscreen(){
        canvas.renderMode = RenderMode.WorldSpace;
        rectTransform.position = defaultPosition;
        rectTransform.rotation = defaultRotation;
        rectTransform.localScale = defaultScale;
        rectTransform.sizeDelta = defaultSize;
        RayCastOff();
        closeButton.SetActive(false);
        newRecipeButton.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerInput.enabled = true;
    }

    public void NewRecipe(){
        RayCastOff();
        newRecipeCanvas.Show();
    }

    public void AddNewRecipe(Recipe newRecipe){
        allRecipes.Add(newRecipe);
        actualRecipe = allRecipes.Count - 1;
        UpdateCanvas();
    }
}
