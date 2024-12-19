using UnityEngine;
using System.Collections;
using System;
using UnityEditor.UI;
using UnityEngine.UI;
using System.Collections.Generic;

public class InteractableObjects : MonoBehaviour
{
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

    public string GetInteractrionText(){
        return interactionText;
    }

    public virtual bool GetCanBeInteracted(int handId){return true;}//Defined in chil

    public void ActivateOutline(bool _activate){
        if(_activate){
            outline.OutlineWidth = 5f;
            return;
        }
        outline.OutlineWidth = 0f;
    }

    public virtual void Interact(int interactionHand){}//Defined in child
    
    protected IEnumerator Coroutine_WaitThenLog(float _duration, Action _callback)
    {
        yield return new WaitForSeconds(_duration);
        _callback?.Invoke();
    }

    public virtual void SetInteractText(){}//Defined in child

    public virtual void UpdateAndShowInteractionMenu(InterractionCanvas interactionMenuManager){
        interactionMenuManager.MoveTo(this);
        ChangeInteractionMenuText(interactionMenuManager);
        ChangeButtonsAction(interactionMenuManager, this is Placard ? true : false);
        ChangeButtonsInteractable(interactionMenuManager);
        interactionMenuManager.SetMenuActive(true);
    }

    public virtual void ChangeInteractionMenuText(InterractionCanvas interactionMenuManager){}//Defined in child

    public void ChangeButtonsAction(InterractionCanvas interactionMenuManager, bool singleButton = false){
        if(singleButton){
            Button button = interactionMenuManager.GetOpenButton();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener( () => Interact(0));
            button.gameObject.SetActive(true);
            foreach(Button otherButton in interactionMenuManager.GetButtons()){
                otherButton.gameObject.SetActive(false);
            }
            return;
        }
        for(int i = 0; i < interactionMenuManager.GetButtons().Count; i++){
            Button button = interactionMenuManager.GetButtons()[i];
            button.onClick.RemoveAllListeners();
            int handid = i;
            button.onClick.AddListener( () => Interact(handid));
            button.onClick.AddListener( () => ChangeButtonsInteractable(interactionMenuManager));
            button.gameObject.SetActive(true);
        }
        interactionMenuManager.GetOpenButton().gameObject.SetActive(false);
    }

    public void ChangeButtonsInteractable(InterractionCanvas interactionMenuManager){
        List<bool> interactable = new();
         for(int i = 0; i < interactionMenuManager.GetButtons().Count; i++){
            interactable.Add(GetCanBeInteracted(i));
         }
         interactionMenuManager.SetButonInteractable(interactable);
    }
}