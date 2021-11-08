using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Objects
{
    public class DestructableObject : MonoBehaviour
    {
        private Animator anim;
        public bool playerInRange = false;

        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
            this.tag = "breakable";

        }

        public void Smash()
        {
            anim.SetBool("smash", true);
            StartCoroutine(breakCo());
        }

        IEnumerator breakCo()
        {
            yield return new WaitForSeconds(.3f);
            this.gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<RPG.Control.PlayerController>().canInteract = true;
                playerInRange = true;
                other.GetComponent<RPG.Control.PlayerController>().interactButton.SetActive(true);
            }
        }

        private void OnTriggerStay2D(Collider2D other) {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<RPG.Control.PlayerController>().canInteract = true;
                playerInRange = true;
                other.GetComponent<RPG.Control.PlayerController>().interactButton.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<RPG.Control.PlayerController>().canInteract = false;
                playerInRange = false;
                other.GetComponent<RPG.Control.PlayerController>().interactButton.SetActive(false);
            }
        }

    }
}

