using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using UnityEngine.SceneManagement;

namespace RPG.Inventories
{
    /// <summary>
    /// To be placed on anything that wishes to drop pickups into the world.
    /// Tracks the drops for saving and restoring.
    /// </summary>
    public class ItemDropper : MonoBehaviour, ISaveable
    {
        // STATE
        private List<Pickup> currentSceneDroppedItems = new List<Pickup>();
        private Dictionary<int, List<DropRecord>> droppedItemsRecords = new Dictionary<int, List<DropRecord>>();

        // PUBLIC
        /// <summary>
        /// Create a pickup at the current position.
        /// </summary>
        /// <param name="item">The item type for the pickup.</param>
        /// <param name="number">
        /// The number of items contained in the pickup. Only used if the item
        /// is stackable.
        /// </param>
        public void DropItem(InventoryItem item, int number)
        {
            SpawnPickup(item, GetDropLocation(), number);
        }

        /// <summary>
        /// Create a pickup at the current position.
        /// </summary>
        /// <param name="item">The item type for the pickup.</param>
        public void DropItem(InventoryItem item)
        {
            SpawnPickup(item, GetDropLocation(), 1);
        }

        // PROTECTED

        /// <summary>
        /// Override to set a custom method for locating a drop.
        /// </summary>
        /// <returns>The location the drop should be spawned.</returns>
        protected virtual Vector3 GetDropLocation()
        {
            return transform.position;
        }

        // PRIVATE

        public void SpawnPickup(InventoryItem item, Vector3 spawnLocation, int number)
        {
            Pickup pickup = item.SpawnPickup(spawnLocation, number);
            currentSceneDroppedItems.Add(pickup);
            Destroy(pickup.gameObject, 25);
        }

        [System.Serializable]
        private struct DropRecord
        {
            public string itemID;
            public SerializableVector3 position;
            public int number;
        }

        object ISaveable.CaptureState()
        {
            RemoveDestroyedDrops();
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            List<DropRecord> newDropRecords = new List<DropRecord>();
            foreach (Pickup pickup in currentSceneDroppedItems)
            {
                DropRecord droppedItem = new DropRecord();
                droppedItem.itemID = pickup.GetItem().GetItemID();
                droppedItem.position = new SerializableVector3(pickup.transform.position);
                droppedItem.number = pickup.GetNumber();
                newDropRecords.Add(droppedItem);
            }
            droppedItemsRecords[currentSceneIndex] = newDropRecords;
            return droppedItemsRecords;
        }

        void ISaveable.RestoreState(object state)
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            Dictionary<int, List<DropRecord>> droppedItems = (Dictionary<int, List<DropRecord>>)state;
            if (!droppedItems.ContainsKey(currentSceneIndex))
            {
                droppedItemsRecords = droppedItems;
                return;
            }

            // Spawn Pickups for given Scene
            foreach (DropRecord dropRecord in droppedItems[currentSceneIndex])
            {
                InventoryItem pickupItem = InventoryItem.GetFromID(dropRecord.itemID);
                Vector3 position = dropRecord.position.ToVector();
                int amount = dropRecord.number;
                SpawnPickup(pickupItem, position, amount);
            }
            droppedItemsRecords = droppedItems;
        }

        /// <summary>
        /// Remove any drops in the world that have subsequently been picked up.
        /// </summary>
        private void RemoveDestroyedDrops()
        {
            List<Pickup> newDroppedItemsList = new List<Pickup>();
            foreach (Pickup pickup in currentSceneDroppedItems)
            {
                if (pickup != null)
                {
                    newDroppedItemsList.Add(pickup);
                }
            }
            currentSceneDroppedItems = newDroppedItemsList;
        }
    }
}