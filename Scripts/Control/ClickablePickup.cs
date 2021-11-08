using UnityEngine;
using RPG.Inventories;
using RPG.Control;
using RPG.Combat;

namespace InventoryExample.Control
{
    [RequireComponent(typeof(Pickup))]
    public class ClickablePickup : MonoBehaviour
    {
        Pickup pickup;

        private void Awake()
        {
            pickup = GetComponent<Pickup>();
        }


        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                pickup.PickupItem();
            }
            // return true;

            if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                pickup.PickupItem();
            }
            return true;
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerController>().interactButton.SetActive(true);
            }
        }

        private void OnTriggerStay2D(Collider2D other) {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerController>().interactButton.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerController>().interactButton.SetActive(false);
            }
        }
    }
}