using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class InterractionCanvas : MonoBehaviour
{
    [SerializeField] TMP_Text interractionText;
    [SerializeField] Button leftHandButton;
    [SerializeField] Button rightHandButton;

    void Awake(){
        gameObject.SetActive(false);
    }

    void Update(){
        gameObject.transform.LookAt(Camera.main.transform);
    }

    public void MoveTo(InteractableObjects interactableObject){
        Vector3 endPos = interactableObject.gameObject.transform.position;
        if(interactableObject is Food or Plate) endPos.y += 1f;
        else if(interactableObject is Knife or Pot) endPos.y += 1.2f;
        else if(interactableObject is Bin) endPos.y += 2.3f;
        gameObject.transform.position = endPos;
    }

    public void SetInteractionText(string _text){
        interractionText.text = _text;
    }

    public List<Button> GetButtons(){
        List<Button> buttonList = new() {leftHandButton,rightHandButton};
        return buttonList;
    }

    public Button GetLeftHandButton(){
        return leftHandButton;
    }
    public Button GetRightHandButton(){
        return leftHandButton;
    }

    public void SetMenuActive(bool _active){
        gameObject.SetActive(_active);
    }

    public void SetButonInteractable(List<bool> _interactable){
        leftHandButton.interactable = _interactable[0];
        rightHandButton.interactable = _interactable[1];
    }
}
