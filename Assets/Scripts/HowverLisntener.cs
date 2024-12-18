using UnityEngine;
using UnityEngine.EventSystems;

public class HoverListener : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public delegate void HoverDelegate();
    public HoverDelegate onHoverEnter;
    public HoverDelegate onHoverExit;

    public void OnPointerEnter(PointerEventData eventData)
    {
        onHoverEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onHoverExit?.Invoke();
    }
}
