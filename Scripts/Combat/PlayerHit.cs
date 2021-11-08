using RPG.Objects;
using UnityEngine;

namespace RPG.Combat
{
    public class PlayerHit : MonoBehaviour
    {

        private void OnTriggerEnter2D(Collider2D other)
        {
            float damage = this.gameObject.GetComponentInParent<RPG.Combat.Fighter>().damage;

            if (other.gameObject.CompareTag("Enemy"))
            {
                other.GetComponent<Enemy>().TakeDamage(damage);
                
            }

        }
    }

}