using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using RPG.Control;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using RPG.Inventories;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, ISaveable
    {
        [Tooltip("Time after which the enemy respawn.")]
        public float deathTime = 5;
        public float dst = 0; //dst == death Since Time
        private float timeBtwnAttack;
        [HideInInspector]
        Transform player;
        [Header("Attack Buttons")]
        [Tooltip("The button used to perform the normal attack")]
        [SerializeField] Button attackButton;
        [SerializeField] Button Skill1Button;
        [SerializeField] Button Skill2Button;
        [SerializeField] Button Skill3Button;
        [SerializeField] 
        [HideInInspector]
        public Vector3 startPosition; //set the starting/respawn position
        [SerializeField] StatsEquipableItem defaultWeapon = null;
        public StatsEquipableItem currentWeapon = null;
        private int rangeAttack = 5;
        [SerializeField] public float damage; //should be weapon damage + str value + modifiers

        Equipment equipment;

        [Header("Variables for melee attack")]
        public Transform attackPos; // where to attack
        public float attackRadius = 3; //for multi-target attacks(skills)

        [Header("Variables for ranged attack")]
        public float checkRadius; //the radius in which we search for GameObjects
        public LayerMask targetToAttack; //get only one type of GameObjects.

        [Header("Skill cooldowns")]
        private float skill1Cooldown = 5;
        private float skill2Cooldown = 10;
        private float skill3Cooldown = 20;
        private float skill1Cool;
        private float skill2Cool;
        private float skill3Cool;

        [Header("Skills damage")]
        public int skill1damage = 35;
        public int skill2damage = 73;
        public int skill3damage = 120;

        [Header("Skills mana cost")]
        private int skill1ManaCost = 35;
        private int skill2ManaCost = 73;
        private int skill3ManaCost = 120;

        public enum ButtonType
        {
            Sword,
            Bow,
            Staff,
    
            Pickup,
            Open,
            Speak,
            Mine,
            Gather,
        }

        [System.Serializable]
        struct ButtonMapping
        {
            public ButtonType type;
            public Sprite texture;
        }

        [SerializeField] ButtonMapping[] buttonMappings = null;

        private void Awake() {
            equipment = GetComponent<Equipment>();
            if(equipment)
            {
                equipment.equipmentUpdated += UpdateWeapon;
            }
        }

        private void Start() 
        {
            startPosition = transform.position;
            timeBtwnAttack = 0;
            skill1Cool = 0;
            skill2Cool = 0;
            skill3Cool = 0;

            if(currentWeapon == null)
            {
                EquipWeapon(defaultWeapon);
            }

            attackButton.onClick.AddListener(() => Attack());
            Skill1Button.onClick.AddListener(() => Skill1());
            Skill2Button.onClick.AddListener(() => Skill2());
            Skill3Button.onClick.AddListener(() => Skill3());
        }
        
        private void Update()
        {
            player = this.transform;
            UpdateCooldowns();

            CheckTypeOfWeapon();

            UpdateDamage();
        }        

        public void Attack()
        {
            if (timeBtwnAttack <= 0)
            {
                if (!currentWeapon.bowAttack && !currentWeapon.staffAttack)
                {
                    GetComponent<PlayerController>().anim.Play("Warrior_normal_attack");

                    timeBtwnAttack = currentWeapon.attackSpeed;
                }
                else
                {
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, rangeAttack, targetToAttack);
                    Array.Sort(colliders, new DistanceComparer(transform));

                    if (colliders.Length >= 1)
                    {
                        if(!colliders[0].CompareTag("Enemy")) return;

                        if (currentWeapon.HasProjectile())
                        {
                            currentWeapon.LaunchProjectile(player, colliders[0], damage);
                            Debug.Log("The arrow was shoot toward " + colliders[0].name);
                        }
                    }
                    else
                    {
                        Debug.Log("No target found in range");
                    }

                    timeBtwnAttack = currentWeapon.attackSpeed;
                }
            }
        }

        private void UpdateDamage()
        {
            damage = GetComponent<PlayerBaseStats>().GetStat(Stat.damage);
        }

        private void CheckTypeOfWeapon() // check the type of weapon and set the correct button
        {
            if (!currentWeapon.bowAttack && !currentWeapon.staffAttack)
            {
                SetButton(attackButton.gameObject, ButtonType.Sword);
            }
            else
            {
                if (currentWeapon.bowAttack)
                {
                    SetButton(attackButton.gameObject, ButtonType.Bow);
                }
                else
                {
                    SetButton(attackButton.gameObject, ButtonType.Staff);
                }
            }
           
        }

        public void SetButton(GameObject button,ButtonType type)
        {
            ButtonMapping mapping = GetButtonMapping(type);
            button.GetComponent<Image>().sprite = mapping.texture; // Setting the button image
        }

        private ButtonMapping GetButtonMapping(ButtonType type)
        {
            foreach (ButtonMapping mapping in buttonMappings)
            {
                if (mapping.type == type)
                {
                    return mapping;
                }
            }
            return buttonMappings[0];
        }

        private void UpdateCooldowns()
        {
            timeBtwnAttack -= Time.deltaTime;
            skill1Cool -= Time.deltaTime;
            skill2Cool -= Time.deltaTime;
            skill3Cool -= Time.deltaTime;
        }

        public void Skill1()
        {            
            if (skill1Cool <= 0)
            {
                if(skill1ManaCost <= GetComponent<PlayerStats>().currentMana)
                {
                    Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRadius, targetToAttack);
                    for (int i = 0; i < enemiesToDamage.Length; i++)
                    {
                        enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(skill1damage);
                    }
                    skill1Cool = skill1Cooldown;
                    GetComponent<PlayerStats>().currentMana -= skill1ManaCost;
                    print("You used the WhirlWIND skill.");
                // foreach (Collider2D item in colliders)
                // {
                //     if(colliders.Length <= 1)
                //     {
                //         colliders[0].GetComponent<Enemy>().TakeDamage(damage);
                //     }
                //     else
                //     {
                //         for (int i = 0; i < colliders.Length; i++)
                //         {
                //             colliders[0].GetComponent<Enemy>().TakeDamage(damage);
                //         }
                //         print("You just attacked: " + item.name);
                //     }
                // }
                }
                else
                {
                    Debug.Log("You don't have enough mana to use the skill.");
                }
            }
        }

        public void Skill2()
        {
            if (skill2Cool <= 0 )
            {
                if(skill2ManaCost <= GetComponent<PlayerStats>().currentMana)
                {
                    Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRadius, targetToAttack);
                    for (int i = 0; i < enemiesToDamage.Length; i++)
                    {
                        enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(skill2damage);
                    }
                    skill2Cool = skill2Cooldown;
                    GetComponent<PlayerStats>().currentMana -= skill2ManaCost;
                    Debug.Log("You used your second skill");
                }
                else
                {
                    Debug.Log("You don't have enough mana to use the skill.");
                }            }            
        }

        public void Skill3()
        {
            if (skill3Cool <= 0)
            {
                if(skill3ManaCost <= GetComponent<PlayerStats>().currentMana)
                {
                    Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRadius, targetToAttack);
                    for (int i = 0; i < enemiesToDamage.Length; i++)
                    {
                        enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(skill3damage);
                    }
                    skill3Cool = skill3Cooldown;
                    GetComponent<PlayerStats>().currentMana -= skill3ManaCost;
                    print("You used your third skill");
                }
                else 
                {
                    Debug.Log("You don't have enough mana to use the skill.");
                }
            }
        }

        public void EquipWeapon(StatsEquipableItem weapon)
        {
            currentWeapon = weapon;
        }

        private void UpdateWeapon()
        {
            var weapon = equipment.GetItemInSlot(EquipLocation.Weapon) as StatsEquipableItem;
            if (weapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
            else
            {
                EquipWeapon(weapon);
            }
        }


        private void OnDrawGizmosSelected() //drawing the hitbox
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPos.position, attackRadius); //drawing the attack radius with red
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, checkRadius);//drawing the radius in which we check for enemies
        }

        
        public object CaptureState()
        {
            // Dictionary<string, object> data = new Dictionary<string, object>();
            // data["currentWeapon"] = currentWeapon.name;
            // return data;

            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            // Dictionary<string, object> data = (Dictionary<string, object>)state;
            // currentWeapon.name = (string)data["currentWeapon"];
            // Debug.Log("Loading the Player equipped weapon");
            string weaponName = (string)state;
            StatsEquipableItem weapon = Resources.Load<StatsEquipableItem>(weaponName);
            EquipWeapon(weapon);
        }

        
        
    }
}
