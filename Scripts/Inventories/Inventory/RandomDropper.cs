using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Stats;
using UnityEngine.AI;

namespace RPG.Inventories
{
    public class RandomDropper : ItemDropper
    {
        // CONFIG DATA
        private Vector3 scatterDistance; //How far can the pickups be scattered from the dropper
        [Tooltip("The list from which items are randomly dropped.")]
        [SerializeField] DropLibrary dropLibrary;

        // CONFIG CHEST DROPS
        [Tooltip("The list from which items are spawn from the chest the script is attached to")]
        [SerializeField] InventoryItem[] chestDropLibrary;
        [SerializeField] bool isQuestDrop = false;
        [SerializeField] int minNumOfDrops;
        [SerializeField] int maxNumOfDrops;

        
        // CONSTANTS
        const int ATTEMPTS = 30;

        private void Awake() {
            scatterDistance = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
        }

        public void RandomDrop()
        {
            if (!this.gameObject.CompareTag("Interactable"))
            {
                var baseStats = GetComponent<EnemyBaseStats>();

                var drops = dropLibrary.GetRandomDrops(baseStats.enemyLevel);
                foreach (var drop in drops)
                {
                    DropItem(drop.item, drop.number);
                }
            }
            else
            {
                ChestRandomDrop();
            }
        }

        protected override Vector3 GetDropLocation()
        {
            scatterDistance = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), Random.Range(-2f, 2f));

            // Debug.Log("Drop location from " + gameObject.name + " is: " + transform.position);
            // Debug.Log("Scatter distance is " + scatterDistance);
            return transform.position + scatterDistance;
        }
        

        private void ChestRandomDrop()
        {
            int numberOfDrops = Random.Range(minNumOfDrops,maxNumOfDrops);
            if (!isQuestDrop)
            {
                for (int i = 0; i < numberOfDrops; i++)
                {
                    var item = chestDropLibrary[Random.Range(0, chestDropLibrary.Length)];
                    DropItem(item, 1);
                }
            }
            else
            {
                for (int i = 0; i < numberOfDrops; i++)
                {
                    var item = chestDropLibrary[i];
                    DropItem(item, 1);
                }
            }
            
        }
    }

}

