using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Control;
using RPG.Inventories;
using UnityEngine;


namespace RPG.Objects
{
    public class Interactable : MonoBehaviour
    {
        public Animator anim;
        public float radius = 3f;
        public bool playerInRange = false;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            this.tag = "Interactable";
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        public virtual void Interact()
        {
            //play open animation
            anim.SetBool("open", true);
            //spawn items in radius
            // GetComponent<RandomDropper>().RandomDrop();
            //maybe give some exp

            //keep the chest anim being opened

            // Destroy the game object
            // Destroy(gameObject);
        }
    }

}
