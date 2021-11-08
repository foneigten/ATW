using System.Collections;
using RPG.Inventories;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] StatsEquipableItem weapon = null;
        [SerializeField] float healthToRestore = 0;
        [SerializeField] float respawnTime = 5f;

        private void OnEnable() {
            tag = "Pickup";
        }

        private void OnDisable() {
            tag = "Untagged";
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                if (weapon != null)
                {
                    other.GetComponent<RPG.Combat.Fighter>().EquipWeapon(weapon);
                }
                else
                {
                    other.GetComponent<RPG.Core.PlayerStats>().currentHealth += healthToRestore;
                }

                StartCoroutine(HideForSeconds(respawnTime));
                // Destroy(gameObject);
            }
        }

        public IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);

            yield return new WaitForSeconds(seconds);

            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
            GetComponent<Collider2D>().enabled = shouldShow;
            GetComponent<SpriteRenderer>().enabled = shouldShow;
        }

    }
}