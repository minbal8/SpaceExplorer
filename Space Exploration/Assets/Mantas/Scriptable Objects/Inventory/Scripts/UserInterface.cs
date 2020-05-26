using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class UserInterface : MonoBehaviour
{

    public PlayerController player;
    private PointerEventData _dragPointerData;

    public InventoryObject inventory;

    public bool isInventoryShown = true;
    public bool isToggable = true;
    public KeyCode toggleKey;



    public Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            inventory.Container.Items[i].parent = this;
        }

        CreateSlots();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSlots();
        CheckInventoryToggle();
    }

    public void UpdateSlots()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in itemsDisplayed)
        {
            if (_slot.Value.ID >= 0)
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[_slot.Value.item.Id].uiDisplay;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.transform.Find("Amount").GetComponent<TextMeshProUGUI>().text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
                _slot.Key.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = _slot.Value.item.Name;
            }
            else
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                _slot.Key.transform.Find("Amount").GetComponent<TextMeshProUGUI>().text = "";
                _slot.Key.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = "";
            }
        }
    }


    public abstract void CreateSlots();

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnClick(GameObject obj)
    {
        
        if (itemsDisplayed[obj].ID >= 0 && !player.mouseItem.isDragged)
        {
            inventory.database.GetItem[itemsDisplayed[obj].ID].Use();
            itemsDisplayed[obj].amount -= 1;

            if (itemsDisplayed[obj].amount <= 0)
                inventory.RemoveItem(itemsDisplayed[obj].item);
        }

    }

    public void OnEnter(GameObject obj)
    {
        //Debug.Log(itemsDisplayed[obj].item.Id + " " + itemsDisplayed[obj].item.Name);

        player.mouseItem.hoverObj = obj;
        if (itemsDisplayed.ContainsKey(obj))
            player.mouseItem.hoverItem = itemsDisplayed[obj];
    }

    public void OnExit(GameObject obj)
    {
        player.mouseItem.hoverObj = null;
        player.mouseItem.hoverItem = null;
    }


    public void OnEnterInterface(GameObject obj)
    {
        player.mouseItem.ui = obj.GetComponent<UserInterface>();
        GameManager.instance.onInventory = true;

    }

    public void OnExitInterface(GameObject obj)
    {
        player.mouseItem.ui = null;
        GameManager.instance.onInventory = false;
    }

    public void OnDragStart(GameObject obj)
    {
        if (Input.GetMouseButton(0))
            player.mouseItem.buttonDown = MouseItem.MouseButtonDown.Mouse1;
        else if (Input.GetMouseButton(1))
            player.mouseItem.buttonDown = MouseItem.MouseButtonDown.Mouse2;


        var mouseObject = new GameObject();
        var rt = mouseObject.AddComponent<RectTransform>();

        //mouseObject.tag = "ItemDraggedUI"; // Needed to clean up after the inventory is closed
        rt.sizeDelta = new Vector2(35, 35);

        mouseObject.transform.SetParent(transform);

        if (itemsDisplayed[obj].ID >= 0)
        {
            var img = mouseObject.AddComponent<Image>();
            img.sprite = inventory.database.GetItem[itemsDisplayed[obj].ID].uiDisplay;
            img.color = new Color(1, 1, 1, 0.7f);
            img.raycastTarget = false;
            player.mouseItem.isDragged = true;
        }
        player.mouseItem.obj = mouseObject;
        player.mouseItem.item = itemsDisplayed[obj];
    }

    public void OnDragEnd(GameObject obj)
    {
        var itemOnMouse = player.mouseItem;
        var mouseHoverItem = itemOnMouse.hoverItem;
        var mouseHoverObj = itemOnMouse.hoverObj;
        var GetItemObject = inventory.database.GetItem;

        if (itemOnMouse.isDragged && isInventoryShown)
        {
            if (mouseHoverObj != null)
            {
                if (mouseHoverItem.CanPlaceInSlot(GetItemObject[itemsDisplayed[obj].ID]) && (mouseHoverItem.item.Id <= -1 || (mouseHoverItem.item.Id >= 0 && itemsDisplayed[obj].CanPlaceInSlot(GetItemObject[mouseHoverItem.item.Id]))))
                    inventory.MoveItem(itemsDisplayed[obj], mouseHoverItem.parent.itemsDisplayed[itemOnMouse.hoverObj]);
            }
            else if ( itemOnMouse.ui == null )
            {
                if (player.mouseItem.buttonDown == MouseItem.MouseButtonDown.Mouse2)
                {
                    inventory.database.GetItem[itemsDisplayed[obj].ID].Drop(1);
                    itemsDisplayed[obj].amount -= 1;
                    

                    if(itemsDisplayed[obj].amount <= 0)
                        inventory.RemoveItem(itemsDisplayed[obj].item);

                }
                else
                {
                    inventory.database.GetItem[itemsDisplayed[obj].ID].Drop(itemsDisplayed[obj].amount);
                    inventory.RemoveItem(itemsDisplayed[obj].item);
                }
            }
            itemOnMouse.isDragged = false;

        }

        // When finished dragging, destroy the drag GameObject
        Destroy(itemOnMouse.obj); 
        itemOnMouse.item = null;
    }

    public void OnDrag(GameObject obj)
    {
        if (player.mouseItem.obj != null)
        {
            player.mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;

            var img = player.mouseItem.obj.GetComponent<Image>();

            if (player.mouseItem.hoverObj == null && player.mouseItem.ui == null)
                img.color = new Color(1, 0.5f, 0.5f, 0.7f); // Will be dropped if pointer is up
            else
                img.color = new Color(1, 1f, 1f, 0.7f);

            //mouseItem.
        }
    }

    void CheckInventoryToggle()
    {
        if (isToggable)
        {
            if (Input.GetKeyDown(toggleKey))
                isInventoryShown = !isInventoryShown;

            transform.GetChild(0).gameObject.SetActive(isInventoryShown);

            if (!isInventoryShown)
            { 
                player.mouseItem.isDragged = false;
                Destroy(player.mouseItem.obj);
            }
        }

    }
}

public class MouseItem
{
    public UserInterface ui;
    public GameObject obj;
    public InventorySlot item;
    public InventorySlot hoverItem;
    public GameObject hoverObj;
    public bool isDragged;
    public MouseButtonDown buttonDown;
    
    public enum MouseButtonDown
    {
        Mouse1,
        Mouse2,
        Mouse3
    }
}

//DOESN'T WORK
//#if UNITY_EDITOR
//[CustomEditor(typeof(UserInterface))]
//public class RandomScript_Editor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector(); // for other non-HideInInspector fields

//        UserInterface script = (UserInterface)target;

//        // draw checkbox for the bool
//        script.isToggable = EditorGUILayout.Toggle("Is Toggable", script.isToggable);
//        if (script.isToggable) // if bool is true, show other fields
//        {
//            script.toggleKey = (KeyCode)EditorGUILayout.EnumPopup("Toggle key", script.toggleKey);
//        }
//    }
//}

//#endif

