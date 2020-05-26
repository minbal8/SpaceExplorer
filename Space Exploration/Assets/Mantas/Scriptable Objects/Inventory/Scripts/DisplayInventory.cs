using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DisplayInventory : MonoBehaviour
{
    public MouseItem mouseItem = new MouseItem();
    private PointerEventData _dragPointerData;

    public GameObject inventoryPrefab;
    public InventoryObject inventory;

    public bool isInventoryShown = false;

    public int X_START;
    public int Y_START;
    public int X_SPACE_BETWEEN_ITEM;
    public int NUMBER_OF_COLUMNS;
    public int Y_SPACE_BETWEEN_ITEMS;

    Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

    // Start is called before the first frame update
    void Start()
    {
        CreateSlots();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSlots();
        CheckInventoryToggle();
    }

    public void UpdateSlots()
    {
        foreach(KeyValuePair<GameObject, InventorySlot> _slot in itemsDisplayed)
        {
            if(_slot.Value.ID >= 0)
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


    public void CreateSlots()
    {
        itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform.GetChild(0).GetChild(0));
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerClick, delegate { OnClick(obj); });
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });
            
            itemsDisplayed.Add(obj, inventory.Container.Items[i]);
        }
        
    }

    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnClick(GameObject obj)
    {
        if (itemsDisplayed[obj].ID >= 0 && !mouseItem.isDragged)
        {
            inventory.database.GetItem[itemsDisplayed[obj].ID].Use();
            itemsDisplayed[obj].amount -= 1;

            if (itemsDisplayed[obj].amount <= 0)
                inventory.RemoveItem(itemsDisplayed[obj].item);
        }
        
    }

    public void OnEnter(GameObject obj)
    {
        mouseItem.hoverObj = obj;
        if (itemsDisplayed.ContainsKey(obj))
            mouseItem.hoverItem = itemsDisplayed[obj];
    }

    public void OnExit(GameObject obj)
    {
        mouseItem.hoverObj = null;
        mouseItem.hoverItem = null;
    }

    public void OnDragStart(GameObject obj)
    {
        var mouseObject = new GameObject();
        var rt = mouseObject.AddComponent<RectTransform>();

        mouseObject.tag = "ItemDraggedUI"; // Needed to clean up after the inventory is closed
        rt.sizeDelta = new Vector2(35, 35);

        mouseObject.transform.SetParent(transform.parent);

        if(itemsDisplayed[obj].ID >= 0)
        {
            var img = mouseObject.AddComponent<Image>();
            img.sprite = inventory.database.GetItem[itemsDisplayed[obj].ID].uiDisplay;
            img.color = new Color(1, 1, 1, 0.7f);
            img.raycastTarget = false;
            mouseItem.isDragged = true;
        }
        mouseItem.obj = mouseObject;
        mouseItem.item = itemsDisplayed[obj];
    }

    public void OnDragEnd(GameObject obj)
    {
        if (mouseItem.isDragged && isInventoryShown)
        {
            if (mouseItem.hoverObj)
            {
                inventory.MoveItem(itemsDisplayed[obj], itemsDisplayed[mouseItem.hoverObj]);
            }
            else
            {
                inventory.database.GetItem[itemsDisplayed[obj].ID].Drop(itemsDisplayed[obj].amount);
                inventory.RemoveItem(itemsDisplayed[obj].item);
            }
            mouseItem.isDragged = false;
            Destroy(mouseItem.obj);
            mouseItem.item = null;
        }
    }

    public void OnDrag(GameObject obj)
    {
        if (mouseItem.obj != null)
        {
            mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;

            var img = mouseItem.obj.GetComponent<Image>();

            if (mouseItem.hoverObj == null)
                img.color = new Color(1, 0.5f, 0.5f, 0.7f); // Will be dropped if pointer is up
            else
                img.color = new Color(1, 1f, 1f, 0.7f);

            //mouseItem.
        }
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMNS)), Y_START + (-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_OF_COLUMNS)), 0f);
    }

    void CheckInventoryToggle()
    {
        if (Input.GetKeyDown(KeyCode.I))
            isInventoryShown = !isInventoryShown;

        transform.GetChild(0).gameObject.SetActive(isInventoryShown);

        if (isInventoryShown)
            Cursor.lockState = CursorLockMode.None;
        else
        {
            mouseItem.isDragged = false;
            Destroy(mouseItem.obj);          
            Cursor.lockState = CursorLockMode.Locked;
        }

    }
}

