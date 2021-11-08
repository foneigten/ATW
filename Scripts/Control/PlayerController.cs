using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using UnityEngine;
using RPG.Saving;
using RPG.Inventories;
using UnityEngine.EventSystems;
using System.Linq;
using RPG.Dialogue;
// using UnityEngine.InputSystem.OnScreen;

namespace RPG.Control
{
    // public enum PlayerState
    // {
    //     walk,
    //     attack,
    //     interact
    // }

    public class PlayerController : MonoBehaviour, ISaveable
    {

        // On-Hud buttons
        [Header("The On-Hud controlls")]
        public MovementJoystick movementJoystick;
        [SerializeField]public GameObject hudButtons;
        [SerializeField]public GameObject interactButton;
        [SerializeField]public GameObject settingsButton;
        [SerializeField] GameObject dialogueWindow;
        Fighter fighter;

        private float animFixDiagValue; // Value needed to differentiate the direction looking on the diagonal walking.

        public bool canInteract = false;
        public Animator anim;
        private Rigidbody2D rb;
        private PlayerStats playerStats;

        private float directionVar = 3.5f; //the float used to get the switching animation on diagonal direction

        bool isDraggingUI = false;


        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            interactButton.SetActive(false); //deactivate the interact button

            this.gameObject.tag = "Player"; // Set the tag of the gameObject to "PLAYER"
            rb = GetComponent<Rigidbody2D>(); // get the Rigitbody2d component where the script is attached
            anim = GetComponent<Animator>(); // get the Animator component where the script is attached

            playerStats = GetComponent<PlayerStats>();

        }



        private void Update() {
            MovementAnimation(); //animate the move of the character

        }

        void FixedUpdate()
        {
            if (movementJoystick.joystickVec.y != 0) //|| movementJoystick.joystickVec.x != 0)
            {

                rb.velocity = new Vector2(movementJoystick.joystickVec.x * playerStats.moveSpeed, movementJoystick.joystickVec.y * playerStats.moveSpeed);
            }
            else
            {
                if (GetComponent<PlayerStats>().currentHealth > 0)
                {
                    rb.velocity = Vector2.zero;
                }
            }

            

            if (InteractWithUI()) return; 

        }

        private bool InteractWithUI()
        {
            if (Input.GetMouseButtonUp(0))
            {
                isDraggingUI = false;
            }
#if UNITY_ANDROID
            if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
#elif UNITY_IOS
            if(Input.touchCount>0 && EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
#else
            if (EventSystem.current.IsPointerOverGameObject())
#endif
            {
                if (Input.GetMouseButtonDown(0))
                {
                    isDraggingUI = true;
                }
                return true;
            }
            if (isDraggingUI)
            {
                return true;
            }
            return false;
        }

        private void MovementAnimation()
        {
            //set the movement value to the animator parameters
            anim.SetFloat("moveX", rb.velocity.x);
            anim.SetFloat("moveY", rb.velocity.y);


            //determin the position where the player look when idle based on the last moving direction
            if(anim.GetFloat("moveY") > anim.GetFloat("moveX") && anim.GetFloat("moveY") > directionVar) //(rb.velocity.y > rb.velocity.x)
            {
                anim.SetFloat("lastMoveY", 1);
                anim.SetFloat("lastMoveX", 0);
            }else if (anim.GetFloat("moveY") < anim.GetFloat("moveX") && anim.GetFloat("moveY") < -directionVar) //(rb.velocity.y < rb.velocity.x)
            {
                anim.SetFloat("lastMoveY", -1);
                anim.SetFloat("lastMoveX", 0);
            }else if(anim.GetFloat("moveX") > anim.GetFloat("moveY") && anim.GetFloat("moveX") > directionVar) //(rb.velocity.x > rb.velocity.y)
            {
                anim.SetFloat("lastMoveX", 1);
                anim.SetFloat("lastMoveY", 0);
            }else if (anim.GetFloat("moveX") < anim.GetFloat("moveY") && anim.GetFloat("moveX") < -directionVar) //(rb.velocity.x < rb.velocity.y)
            {
                anim.SetFloat("lastMoveX", -1);
                anim.SetFloat("lastMoveY", 0);
            }
        }    

        public void PlayerIsDead()
        {
            // Play death animation

            // Disable the On-HUD controls and general controls.
            movementJoystick.joystickVec.y = 0;
            if(hudButtons.activeInHierarchy)
            {
                hudButtons.SetActive(false);
            }
            rb.isKinematic = true;
            //stops the movement
            rb.bodyType = RigidbodyType2D.Static;
        }

        public void Respawn()
        {
            this.gameObject.transform.position = GetComponent<Fighter>().startPosition;

            // Re-enable the On-HUD controls and general controls.
            hudButtons.SetActive(true);
            rb.isKinematic = false;
            rb.bodyType = RigidbodyType2D.Dynamic;

            GetComponent<Fighter>().deathTime = 10;
        }

        public void Interact()
        {
            Collider2D[] objectsAroundPlayer = Physics2D.OverlapCircleAll(transform.position, .5f); //the player being the first object (0)
            Array.Sort(objectsAroundPlayer, new DistanceComparer(transform));

            for (int i = 0; i < objectsAroundPlayer.Count(); i++)
            {
                if (objectsAroundPlayer[i].gameObject.CompareTag("Interactable") || objectsAroundPlayer[i].gameObject.CompareTag("breakable"))
                {
                    if (objectsAroundPlayer[i].gameObject.CompareTag("Interactable"))
                    {
                        objectsAroundPlayer[i].gameObject.GetComponent<Objects.Interactable>().Interact();
                        return;
                    }

                    if (objectsAroundPlayer[i].gameObject.CompareTag("breakable"))
                    {
                        objectsAroundPlayer[i].gameObject.GetComponent<Objects.DestructableObject>().Smash();
                        return;
                    }
                }

                if (objectsAroundPlayer[i].gameObject.CompareTag("Pickup"))
                {
                    objectsAroundPlayer[i].gameObject.GetComponent<Pickup>().PickupItem();
                }

                if (objectsAroundPlayer[i].gameObject.CompareTag("Quest"))
                {
                    objectsAroundPlayer[i].gameObject.GetComponent<AIConversant>().StartDialogue();
                    interactButton.SetActive(false);
                }
            }

        }

        private void OnTriggerEnter2D(Collider2D other) {
            if(!other.CompareTag("Enemy"))
            {
                if (other.GetComponent<Pickup>())
                {
                    fighter.SetButton(interactButton, Fighter.ButtonType.Pickup);
                    interactButton.SetActive(true);
                }
                if (other.GetComponent<TreasureChest>() && other.CompareTag("Interactable"))
                {
                    fighter.SetButton(interactButton, Fighter.ButtonType.Open);
                    interactButton.SetActive(true);
                }
                if (other.CompareTag("NPC") || other.CompareTag("Quest"))
                {
                    fighter.SetButton(interactButton, Fighter.ButtonType.Speak);
                    interactButton.SetActive(true);
                }
                if (other.CompareTag("breakable"))
                {
                    fighter.SetButton(interactButton, Fighter.ButtonType.Open);
                    interactButton.SetActive(true);
                }
                

            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (this.CompareTag("Player"))
            {
                interactButton.SetActive(false);
            }
        }

        public object CaptureState()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["position"] = new SerializableVector3(transform.position);
            return data;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> data = (Dictionary<string, object>)state;
            transform.position = ((SerializableVector3)data["position"]).ToVector();
        }
    }
}