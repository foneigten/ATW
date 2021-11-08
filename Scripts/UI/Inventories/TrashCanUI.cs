using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using RPG.Inventories;
using RPG.Core.UI.Dragging;

namespace RPG.UI.Inventories
{
    public class TrashCanUI : MonoBehaviour, IDragContainer<InventoryItem>
    {
        // CONFIG DATA
        // STATE
        int index;
        InventoryItem item;
        Inventory inventory;

        // PUBLIC
        public int MaxAcceptable(InventoryItem item)
        {
            return int.MaxValue;
        }

        public void AddItems(InventoryItem item, int number)
        {
        }

        public int GetNumber()
        {
            return 0;
        }

        public void RemoveItems(int number)
        {
        }

        public InventoryItem GetItem()
        {
            return null;
        }
    }
}