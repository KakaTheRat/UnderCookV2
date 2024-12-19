using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Pot : InteractableObjects
{
    [SerializeField] GameObject smokeVfx;
    [SerializeField] Slider slider;

    private GameObject preparingItem;
    
    public override bool GetCanBeInteracted(int handID)
    {
        if(preparingItem == null){
            GameObject holdingItem = playerController.GetHoldingItem(handID);
            if(holdingItem == null)return false;
            IngredientManager ingredientManager = holdingItem.GetComponent<IngredientManager>();
            if(ingredientManager != null && ingredientManager.GetCanBeCook() && !ingredientManager.GetCook()){
                return true;
            }else{return false;}
        }else{
            GameObject holdingItem = playerController.GetHoldingItem(handID);
            if(holdingItem == null)return true;
        }
        return false;
    }

    public override void Interact(int handID)
    {
        if(preparingItem == null){
            Animator animator = GetComponent<Animator>();
            animator.enabled = false;
            preparingItem = playerController.GetHoldingItem(handID);
            preparingItem.transform.SetParent(gameObject.transform);
            // preparingItem.SetActive(false);
            playerController.ReleaseItem(handID);
            preparingItem.transform.SetParent(gameObject.transform);
            preparingItem.transform.localPosition = Vector3.zero;
            slider.interactable = true;
            ChangeInteractionMenuText(FindAnyObjectByType<InterractionCanvas>());
        }else{
            preparingItem.SetActive(true);
            playerController.HoldItem(preparingItem, handID);
            preparingItem = null;
            slider.value = 0f;
            slider.interactable = false;
            SliderMoved(0f);
            ChangeInteractionMenuText(FindAnyObjectByType<InterractionCanvas>());
        }
        
    }

    void Cook(){
        slider.interactable = false;
        Animator animator = GetComponent<Animator>();
        animator.enabled = true;
        animator.SetTrigger("Cook");
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        playerController.Static(true);
        outline.OutlineWidth = 0f;
        StartCoroutine(Coroutine_WaitThenLog(2f, EndCook));
    }

    void EndCook(){
        preparingItem.GetComponent<IngredientManager>().Cook(smokeVfx);
        preparingItem.SetActive(true);
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("Cook");
        playerController.Static(false);
    }

    public override void SetInteractText(){
        interactionText = $"cook";
    }

    public override void ChangeInteractionMenuText(InterractionCanvas interactionMenuManager){
        if(preparingItem == null) interactionMenuManager.SetInteractionText($"Cook the ingredient in your:");
        else {
            string cookStatus;
            IngredientManager ingredientManager = preparingItem.GetComponent<IngredientManager>();
            if(ingredientManager.GetCook()) cookStatus = "cooked";
            else cookStatus = "raw";
            interactionMenuManager.SetInteractionText($"Get back the {cookStatus} {ingredientManager.GetIngredientName()} in your:");
        }
    }

    public void SliderMoved(float sliderValue){
        float requiredPotSize = Mathf.Lerp(80f, 100f, sliderValue);
        gameObject.transform.localScale = new Vector3(requiredPotSize,requiredPotSize,requiredPotSize);
        if(sliderValue == 1f){
            Cook();
        }
    }
}
