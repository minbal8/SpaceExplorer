using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Default Object", menuName = "Inventory System/Items/Default")]
public class DefaultObject : ItemObject
{
    public override void Use()
    {
        throw new System.NotImplementedException();
    }

    void Awake()
    {
        type = ItemType.Default;
    }
}
