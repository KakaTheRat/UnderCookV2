using UnityEngine;
using UnityEngine.UI;

public class Food : InteractableObjects
{
    [SerializeField] private Material mat;
    [SerializeField] GameObject ingredientPrefab;
    private bool canBeCut;
    private bool canBeCook;
    public override bool GetCanBeInteracted(int interactionHand){
        GameObject holdingItem = playerController.GetHoldingItem(interactionHand);
        if(holdingItem == null){
            return true;
        }else{return false;}
    }

    public override void Interact(int interactionHand){
        GameObject clone = Instantiate(ingredientPrefab);
        clone.transform.localScale = transform.localScale; 
        clone.transform.localRotation = transform.localRotation; 
        SetInfos();
        IngredientManager ingredientManager = clone.AddComponent<IngredientManager>();
        ingredientManager.SetAttributes(itemName, canBeCut, canBeCook,mat);
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        playerController.HoldItem(clone, interactionHand);
        Debug.Log(clone.name);
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
}
