using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlacardCanvas : MonoBehaviour
{
    [SerializeField] GameObject Shelfbutton;
    [SerializeField] GameObject FoodImage;
    [SerializeField] Button closeButton;

    [SerializeField] TMP_Text ingredientName;
    [SerializeField] TMP_Text ingredientDescription;

    PlayerInput playerInput;

    void Awake(){
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
    }

    public GameObject GetShelfButton(){
        return Shelfbutton;
    } 
    public GameObject GetFoodImage(){
        return FoodImage;
    } 
    public Button GetCloseButton(){
        return closeButton;
    } 
    public void Show(){
        HideText();
        gameObject.SetActive(true);
        playerInput.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    public void Hide(){
        gameObject.SetActive(false);
        playerInput.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SetIngredientName(string name){
        ingredientName.text = name;
        ingredientName.enabled = true;
    }
    public void SetIngredientDescription(string description){
        ingredientDescription.text = description;
        ingredientDescription.enabled = true;
    }

    public void HideText(){
        ingredientName.enabled = false;
        ingredientDescription.enabled = false;
    }

}
