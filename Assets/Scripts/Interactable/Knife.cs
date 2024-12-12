using UnityEngine;
using UnityEngine.UI;

public class Knife : InteractableObjects
{
    private GameObject preparingItem;
    [SerializeField] GameObject knifeprefab;
    GameObject knife;
    public override bool GetCanBeInteracted()
    {
        if(preparingItem != null)return false;
        GameObject holdingItem = playerController.GetHoldingItem();
        if(holdingItem == null)return false;
        IngredientManager ingredientManager = holdingItem.GetComponent<IngredientManager>();
        if(ingredientManager != null && ingredientManager.GetCanBeCut() && !ingredientManager.GetCut()){
            return true;
        }else{return false;}
    }

    public override void Interact()
    {
        knife = Instantiate(knifeprefab, GameObject.FindGameObjectWithTag("HoldingPlaceHolder").transform);
        knife.transform.localPosition =  new Vector3(-0.00016f, 0.00039f, -0.00229f);
        knife.transform.localRotation =  Quaternion.Euler(-46.421f, 37.352f, -136.495f);
        knife.transform.localScale = new Vector3(1f, 0.8549f,1f);
        preparingItem = playerController.GetHoldingItem();
        playerController.ReleaseItem();
        preparingItem.transform.SetParent(GameObject.FindGameObjectWithTag("CutPlaceHolder").transform);
        preparingItem.transform.localPosition = new Vector3(0f,0f,0f);
        if(preparingItem.name.Contains("Cucumber")){
            preparingItem.transform.localPosition = new Vector3(0f,0.065f,0f); 
        }
        playerController.Static(true);
        playerController.ToggleCutAnim(true);
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        StartCoroutine(Coroutine_WaitThenLog(2f, EndCut));
    }

    private void EndCut()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Stop();
        preparingItem.GetComponent<IngredientManager>().Cut();
        playerController.ToggleCutAnim(false);
        playerController.Static(false);
        Destroy(knife);
        playerController.HoldItem(preparingItem);
        preparingItem = null;
    }

    public override void SetInteractText(){
        GameObject playerHolding = playerController.GetHoldingItem();
        if(playerHolding != null){
            interactionText = $"cut the {playerHolding.GetComponent<IngredientManager>().GetIngredientName()}";
        }
    }

    public override void ChangeInteractionMenuText(InterractionCanvas interactionMenuManager){
        interactionMenuManager.SetInteractionText($"Cut the item in your:");
    }
    public override void ChangeButtonsAction(InterractionCanvas interactionMenuManager){
        foreach(Button button in interactionMenuManager.GetButtons()){
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener( () => Interact());
        }
    }

}
