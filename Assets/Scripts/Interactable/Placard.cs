using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[System.Serializable]
public class ShelfContent{
    public List<Food> food = new();
}

public class Placard : InteractableObjects
{   
    [SerializeField] string placardName;
    [SerializeField] PlacardCanvas placardCanvas;
    [SerializeField] bool magique;
    [SerializeField] List<ShelfContent> placardContent = new();
    List<GameObject> buttonsCreated = new();
    List<GameObject> imageCreated = new();
    
    public override void Interact(int interactionHand = 0){
        if(magique) GeneratePlacardMagic();
        for(int i = 0; i < placardContent.Count ; i++){
            GameObject newButtonGameObject = Instantiate(placardCanvas.GetShelfButton() , placardCanvas.GetShelfButton().transform.parent.transform);
            Button newButton = newButtonGameObject.GetComponent<Button>();
            TMP_Text newButtonText = newButtonGameObject.transform.GetChild(0).GetComponent<TMP_Text>();
            newButtonText.text = $"Shelf {i + 1}";
            newButton.onClick.RemoveAllListeners();
            int shelfIndex = i;
            newButton.onClick.AddListener(() => OpenShelf(shelfIndex));
            newButtonGameObject.SetActive(true);
            buttonsCreated.Add(newButtonGameObject);
        }
        placardCanvas.GetShelfButton().SetActive(false);
        Button closeButton = placardCanvas.GetCloseButton();
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() => Close());
        OpenShelf(0);
    }

    async void OpenShelf(int shelfIndex){
        ClearList(imageCreated);
        for(int i = 0; i < placardContent[shelfIndex].food.Count; i++){
            GameObject newFoodImage = Instantiate(placardCanvas.GetFoodImage() , placardCanvas.GetFoodImage().transform.parent.transform);
            Button newButton = newFoodImage.GetComponent<Button>();
            RawImage newRawImage = newFoodImage.GetComponent<RawImage>();
            Texture2D previewImage = AssetPreview.GetAssetPreview(placardContent[shelfIndex].food[i].GetIngredientPrefab());
            while(previewImage == null){
                previewImage = AssetPreview.GetAssetPreview(placardContent[shelfIndex].food[i].GetIngredientPrefab());
                await Task.Delay(10);
            }
            newRawImage.texture = previewImage;
            newButton.onClick.RemoveAllListeners();
            Food food = placardContent[shelfIndex].food[i];
            newButton.onClick.AddListener(() => GiveFood(food, newFoodImage, shelfIndex));
            HoverListener hoverListener = newFoodImage.AddComponent<HoverListener>();
            hoverListener.onHoverEnter = () => UdateTextInfo(food);
            hoverListener.onHoverExit = () => placardCanvas.HideText();
            newFoodImage.SetActive(true);
            imageCreated.Add(newFoodImage);
        }
        placardCanvas.GetFoodImage().SetActive(false);
        if(placardCanvas.gameObject.activeInHierarchy == false) placardCanvas.Show();
    }

    void  GiveFood(Food foodToGive, GameObject foodImage, int shelfIndex){
        if(playerController.GetHoldingItem(0) == null) foodToGive.Interact(0);
        else if(playerController.GetHoldingItem(1) == null) foodToGive.Interact(1);
        else return;
        int imageIndex = imageCreated.IndexOf(foodImage);
        placardContent[shelfIndex].food.RemoveAt(imageIndex);
        imageCreated.Remove(foodImage);
        Destroy(foodImage);
    }

    void Reset(){
        ClearList(buttonsCreated);
        ClearList(imageCreated);
    }

    void ClearList(List<GameObject> list){
        foreach(GameObject gameObject in list){
            Destroy(gameObject);
        }
        list.Clear();
    }

    void GeneratePlacardMagic(){
        placardContent.Clear();
        List<Food> foodPresets = FindObjectsOfType<Food>().ToList();
        int shelfNumber = Random.Range(1,7);
        for(int i = 0; i < shelfNumber; i++){
            ShelfContent newShelf = new();
            int itemInShelf = Random.Range(0,21);
            for(int j = 0; j < itemInShelf; j++){
                newShelf.food.Add(foodPresets[Random.Range(0,foodPresets.Count)]);
            }
            placardContent.Add(newShelf);
        }
    }

    public override void ChangeInteractionMenuText(InterractionCanvas interactionMenuManager){
        interactionMenuManager.SetInteractionText(placardName);
    }

    void UdateTextInfo(Food food){
        placardCanvas.SetIngredientName(food.GetIngredientName());
        placardCanvas.SetIngredientDescription(food.GetIngredientDescription());
    }

    public string GetName(){
        return placardName;
    }

    void Close(){
        Reset();
        placardCanvas.Hide();
    }
}
