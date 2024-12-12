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
        Quaternion rotation = gameObject.transform.rotation;
        gameObject.transform.rotation = quaternion.Euler(rotation.x, -rotation.y, rotation.z);
    }

    public void MoveTo(GameObject interactableObject){
        Vector3 endPos = interactableObject.transform.position;
        endPos.y += 1f;
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
}
