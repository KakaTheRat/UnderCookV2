using UnityEngine;
using UnityEngine.UI;

public class Pot : InteractableObjects
{
    [SerializeField] GameObject smokeVfx;
    private GameObject preparingItem;
    int lastHoldingHand = 0;
    
    public override bool GetCanBeInteracted(int handID)
    {
        if(preparingItem != null)return false;
        GameObject holdingItem = playerController.GetHoldingItem(handID);
        if(holdingItem == null)return false;
        IngredientManager ingredientManager = holdingItem.GetComponent<IngredientManager>();
        if(ingredientManager != null && ingredientManager.GetCanBeCook() && !ingredientManager.GetCook()){
            return true;
        }else{return false;}
    }

    public override void Interact(int handID)
    {
        preparingItem = playerController.GetHoldingItem(handID);
        preparingItem.transform.SetParent(gameObject.transform);
        preparingItem.SetActive(false);
        playerController.ReleaseItem(handID);
        GetComponent<Animator>().SetTrigger("Cook");
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        playerController.Static(true);
        outline.OutlineWidth = 0f;
        lastHoldingHand = handID;
        StartCoroutine(Coroutine_WaitThenLog(2f, EndCook));
    }

    void EndCook(){
        preparingItem.SetActive(true);
        GetComponent<Animator>().SetTrigger("Cook");
        playerController.Static(false);
        preparingItem.GetComponent<IngredientManager>().Cook(smokeVfx);
        preparingItem.SetActive(true);
        playerController.HoldItem(preparingItem, lastHoldingHand);
        preparingItem = null;
        Debug.Log("FINITO");
    }

    public override void SetInteractText(){
        interactionText = $"cook";
    }

    public override void ChangeInteractionMenuText(InterractionCanvas interactionMenuManager){
        interactionMenuManager.SetInteractionText($"Cook the ingredient in your:");
    }
}
