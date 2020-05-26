using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    None,
    Default,
    Food,
    Resource,
    Material,
    Weapon,
    Helmet,
    Suit,
    OxygenTank,
    Throwable
}

public abstract class ItemObject : ScriptableObject
{
    public int id;
    public Sprite uiDisplay;
    public ItemType type;
    public GameObject groundItemPrefab;
    public GameObject groundStackItemPrefab;

    [TextArea(10, 15)] 
    public string description;

    void Awake()
    {
        groundItemPrefab = ItemSettings.instance.defaultGroundObject;
        groundStackItemPrefab = ItemSettings.instance.defaultGroundObject;
    }


    public virtual void Use() { } // Eat, equip, ect.
    
    // Remove item and drop it on the ground
    public virtual void Drop(int amount = 1) {} 

}

[System.Serializable]
public class Item
{
    public string Name;
    public int Id;

    public Item()
    {
        Name = "";
        Id = -1;
    }

    public Item(ItemObject item)
    {
        Name = item.name;
        Id = item.id;
    }
}