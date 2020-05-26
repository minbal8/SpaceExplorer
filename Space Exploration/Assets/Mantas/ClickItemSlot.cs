using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ClickItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image image;
    private UIItem uiItem;

    void Awake()
    {
        image = GetComponent<Image>();
        uiItem = GetComponent<UIItem>();
        //Debug.Log("Awake");
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //uiItem.inventorySlot.item.Use();
        //uiItem.inventorySlot.amount -= 1;

        //// Open a context menu (use/eat/interact|drop)
        //Debug.Log("Click");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //image.color = new Color(image.color.r, image.color.g, image.color.b, .50f);
        
        //// Display item description
        //Debug.Log("Enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //image.color = new Color(image.color.r, image.color.g, image.color.b, 0.25f);
        //Debug.Log("Exit");
    }
}
