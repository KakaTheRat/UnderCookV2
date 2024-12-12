using UnityEngine;
using System.Collections;
using System;

public class InteractableObjects : MonoBehaviour
{

    [SerializeField] protected string itemName;
    protected string interactionText;

    private GameObject player;
    protected PlayerController playerController;
    protected Outline outline;
    


    void Awake(){
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        outline = gameObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.blue;
        outline.OutlineWidth = 0f;
        SetInteractText();        
    }

    public string GetName(){
        return itemName;
    }

    public string GetInteractrionText(){
        return interactionText;
    }

    public virtual bool GetCanBeInteracted(){return true;}//Defined in chil

    public void ActivateOutline(bool _activate){
        if(_activate){
            outline.OutlineWidth = 5f;
            return;
        }
        outline.OutlineWidth = 0f;
    }

    public virtual void Interact(){}//Defined in child
    
    protected IEnumerator Coroutine_WaitThenLog(float _duration, Action _callback)
    {
        yield return new WaitForSeconds(_duration);
        _callback?.Invoke();
    }

    public virtual void SetInteractText(){}//Defined in child

    public void UpdateAndShowInteractionMenu(InterractionCanvas interactionMenuManager){
        interactionMenuManager.MoveTo(gameObject);
        ChangeInteractionMenuText(interactionMenuManager);
        ChangeButtonsAction(interactionMenuManager);
        interactionMenuManager.SetMenuActive(true);
    }

    public virtual void ChangeInteractionMenuText(InterractionCanvas interactionMenuManager){}//Defined in child 
    public virtual void ChangeButtonsAction(InterractionCanvas interactionMenuManager){}//Defined in child 
}