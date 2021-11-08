using System.Collections;
using System.Collections.Generic;
using RPG.Inventories;
using UnityEngine;

public class InventoryTransaction : MonoBehaviour
{
    [SerializeField] private List<InventoryItem> items;

    public void GiveItems(int numOfItems)
    {
        Inventory inventory = Inventory.GetPlayerInventory();
        foreach (InventoryItem item in items)
        {
            if(item.IsStackable())
            {
                inventory.AddToFirstEmptySlot(item, numOfItems);
            }
            else
            {
                for (int i = 0; i < numOfItems; i++)
                {
                    inventory.AddToFirstEmptySlot(item, 1);
                }
            }

        }
    }

    public void TakeItems(int numOfItems)
    {
        Inventory inventory = Inventory.GetPlayerInventory();
        foreach (InventoryItem item in items)
        {
            if (item.IsStackable())
            {
                inventory.RemoveItem(item, numOfItems);
            }
            else
            {
                for (int i = 0; i < numOfItems; i++)
                {
                    inventory.RemoveItem(item, 1);
                }
            }
            // inventory.RemoveItem(item, numOfItems);
        }
    }
}
