using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Object", menuName = "Inventory System/Items/Equipment")]
public class EquipmentObject : ItemObject
{
    public float speedBonus;
    public float defenceBonus;

    void Awake()
    {
        if(type == ItemType.None)
        {
            type = ItemType.Suit;
        }
  
    }

    public override void Use()
    {
        throw new System.NotImplementedException();
    }

    public override void Drop(int amount)
    {
        PlayerController player = GameManager.instance.player;

        if (player)
        {
            if (amount == 1)
            {
                var droppedItem = Instantiate(groundItemPrefab, player.transform.position + player.transform.forward, Quaternion.Euler(new Vector3(Random.Range(-45, 45), Random.Range(-45, 45), Random.Range(-45, 45))));
                droppedItem.tag = "Item";
                var groundItem = droppedItem.AddComponent<GroundItem>();
                groundItem.item = this;

                return;
            }

            // TODO Dropping multiple items, drops a sack of food (groundItemPrefab -> sackPrefab)
            var droppedItems = Instantiate(groundItemPrefab, player.transform.position + player.transform.forward, Quaternion.Euler(new Vector3(Random.Range(-45, 45), Random.Range(-45, 45), Random.Range(-45, 45))));
            droppedItems.tag = "Item";
            var groundItems = droppedItems.AddComponent<GroundItem>();
            groundItems.amount = amount;
            groundItems.item = this;
        }
    }

}
