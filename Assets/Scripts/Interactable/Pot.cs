using UnityEngine;
using UnityEngine.UI;

public class Pot : InteractableObjects
{
    [SerializeField] GameObject smokeVfx;
    private GameObject preparingItem;
    
    public override bool GetCanBeInteracted()
    {
        if(preparingItem != null)return false;
        GameObject holdingItem = playerController.GetHoldingItem();
        if(holdingItem == null)return false;
        IngredientManager ingredientManager = holdingItem.GetComponent<IngredientManager>();
        if(ingredientManager != null && ingredientManager.GetCanBeCook() && !ingredientManager.GetCook()){
            return true;
        }else{return false;}
    }

    public override void Interact()
    {
        preparingItem = playerController.GetHoldingItem();
        preparingItem.transform.SetParent(gameObject.transform);
        preparingItem.SetActive(false);
        playerController.ReleaseItem();
        GetComponent<Animator>().SetTrigger("Cook");
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        playerController.Static(true);
        outline.OutlineWidth = 0f;
        StartCoroutine(Coroutine_WaitThenLog(2f, EndCook));
    }

    void EndCook(){
        preparingItem.SetActive(true);
        GetComponent<Animator>().SetTrigger("Cook");
        playerController.Static(false);
        preparingItem.GetComponent<IngredientManager>().Cook(smokeVfx);
        preparingItem.SetActive(true);
        playerController.HoldItem(preparingItem);
        preparingItem = null;
        Debug.Log("FINITO");
    }

    public override void SetInteractText(){
        GameObject playerHolding = playerController.GetHoldingItem();
        if(playerHolding != null){
            interactionText = $"cook your {playerHolding.GetComponent<IngredientManager>().GetIngredientName()}";
        }
    }

    public override void ChangeInteractionMenuText(InterractionCanvas interactionMenuManager){
        interactionMenuManager.SetInteractionText($"Cook the ingredient in your:");
    }
    public override void ChangeButtonsAction(InterractionCanvas interactionMenuManager){
        foreach(Button button in interactionMenuManager.GetButtons()){
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener( () => Interact());
        }
    }
}
