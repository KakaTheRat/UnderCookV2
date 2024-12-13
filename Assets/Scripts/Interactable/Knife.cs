using UnityEngine;
using UnityEngine.UI;

public class Knife : InteractableObjects
{
    private GameObject preparingItem;
    [SerializeField] GameObject knifeprefab;
    GameObject knife;
    int lastHoldingHand = 0;
    public override bool GetCanBeInteracted(int handId)
    {
        if(preparingItem != null)return false;
        GameObject holdingItem = playerController.GetHoldingItem(handId);
        if(holdingItem == null)return false;
        IngredientManager ingredientManager = holdingItem.GetComponent<IngredientManager>();
        if(ingredientManager != null && ingredientManager.GetCanBeCut() && !ingredientManager.GetCut()){
            return true;
        }else{return false;}
    }

    public override void Interact(int handId)
    {
        knife = Instantiate(knifeprefab, GameObject.FindGameObjectWithTag("KnifePlaceHolder").transform);
        knife.transform.localPosition =  new Vector3(-0.00016f, 0.00039f, -0.00229f);
        knife.transform.localRotation =  Quaternion.Euler(-46.421f, 37.352f, -136.495f);
        knife.transform.localScale = new Vector3(1f, 0.8549f,1f);
        preparingItem = playerController.GetHoldingItem(handId);
        playerController.ReleaseItem(handId);
        preparingItem.transform.SetParent(GameObject.FindGameObjectWithTag("CutPlaceHolder").transform);
        preparingItem.transform.localPosition = new Vector3(0f,0f,0f);
        if(preparingItem.name.Contains("Cucumber")){
            preparingItem.transform.localPosition = new Vector3(0f,0.065f,0f); 
        }
        playerController.Static(true);
        playerController.ToggleCutAnim(true);
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        lastHoldingHand = handId;
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
        playerController.HoldItem(preparingItem, lastHoldingHand);
        preparingItem = null;
    }

    public override void SetInteractText(){
        interactionText = $"cut";
    }

    public override void ChangeInteractionMenuText(InterractionCanvas interactionMenuManager){
        interactionMenuManager.SetInteractionText($"Cut the item in your:");
    }

}
