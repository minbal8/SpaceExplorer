using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food Object", menuName = "Inventory System/Items/Food")]
public class FoodObject : ItemObject
{
    public float restoreFoodAmount;
    public float restoreHealthAmount;

    void Awake()
    {
        type = ItemType.Food;
        groundStackItemPrefab = ItemSettings.instance.foodGroundStackObject;
    }

    // When food is used, it's eaten
    public override void Use()
    {
        PlayerController player = GameManager.instance.player;
        player.Eat(restoreHealthAmount, restoreFoodAmount); 
    }

    public override void Drop(int amount)
    {
        PlayerController player = FindObjectOfType<PlayerController>();

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
            var droppedItems = Instantiate(groundStackItemPrefab, player.transform.position + player.transform.forward, Quaternion.Euler(new Vector3(Random.Range(-45, 45), Random.Range(-45, 45), Random.Range(-45, 45))));
            droppedItems.tag = "Item";
            var groundItems = droppedItems.AddComponent<GroundItem>();
            groundItems.amount = amount;
            groundItems.item = this;
        }
    }
}
