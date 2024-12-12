using UnityEngine;
using UnityEngine.UI;

public class Food : InteractableObjects
{
    [SerializeField] private Material mat;
    [SerializeField] GameObject ingredientPrefab;
    private bool canBeCut;
    private bool canBeCook;
    public override bool GetCanBeInteracted(){
        GameObject holdingItem = playerController.GetHoldingItem();
        if(holdingItem == null){
            return true;
        }else{return false;}
    }

    public override void Interact(){
        GameObject clone = Instantiate(ingredientPrefab);
        clone.transform.localScale = transform.localScale; 
        SetInfos();
        IngredientManager ingredientManager = clone.AddComponent<IngredientManager>();
        ingredientManager.SetAttributes(itemName, canBeCut, canBeCook,mat);
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        playerController.HoldItem(clone);
    }

    void SetInfos(){
        Ingredient ingredientInfo = FindObjectOfType<JsonManager>().GetIngredient(itemName);
        if(ingredientInfo == null){return;}
        canBeCook = ingredientInfo.canBeCook;
        canBeCut = ingredientInfo.canBeCut;
    }

    public override void SetInteractText(){
        interactionText = $"take {itemName}" ;
    }

    public override void ChangeInteractionMenuText(InterractionCanvas interactionMenuManager){
        interactionMenuManager.SetInteractionText($"Take the {itemName} in your:");
    }
    public override void ChangeButtonsAction(InterractionCanvas interactionMenuManager){
        foreach(Button button in interactionMenuManager.GetButtons()){
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener( () => Interact());
        }
    }
}
