using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Combat;
using RPG.Control;
using RPG.Saving;
using RPG.Inventories;
using RPG.Stats;
using System;

namespace RPG.Core
{
    public class PlayerStats : MonoBehaviour, ISaveable
    {
        Fighter fighter;
        PlayerController playerController;
        PlayerBaseStats playerStats;

        [Header("The city where the player will be spawned on starting the game")]
        // Vector3 defaultSpawnCity = new Vector3(12, -3, 0);

        private float maxHealth;
        private float maxMana;

        [Header("Player Stats")]
        public string playerName;
        public Text playerNameText;
        public int playerLevel;
        [SerializeField] public float currentExperience = 0;
        public float baseHealth; //the health component from the script
        [SerializeField]public float currentHealth;
        public float baseMana;
        [SerializeField]public float currentMana;
        public float armor;
        public float moveSpeed = 5;

        [Header("Player currency")]
        [SerializeField] private float cooper;
        [SerializeField] private float silver;
        [SerializeField] private float gold;
        [SerializeField] private float platinum;

        [Header("Player special currency")]
        [SerializeField] private float blueDiamonds; // can Buy with money or farm in some special dungeons
        [SerializeField] private float manaCrystals;

        [Header("currentHealth and baseMana bar")]
        [SerializeField]private Image currentHealthBar;
        [SerializeField]private Image curentManaBar;
        [SerializeField] private Text currentHealthValue;
        [SerializeField] private Text currentManaValue;

        [Header("Experience bar")]
        [SerializeField]private Image currentExpBar;
        [SerializeField]private Text currentExpValue;
        private float currentExpFill;

        public event Action onExperienceGained;

        float expToReward;


        private void Awake() {
            playerLevel = GetComponent<PlayerBaseStats>().GetLevel();
            playerStats = GetComponent<PlayerBaseStats>();
            fighter = GetComponent<Fighter>();
            playerController = GetComponent<PlayerController>();
            currentHealth = playerStats.GetStat(Stat.Health); // Set the starting health to be equal to baseHealth based on level of progression
            currentMana = playerStats.GetStat(Stat.Mana);   // Set the starting mana to be equal to baseMana based on level of progression
            armor = playerStats.GetStat(Stat.armor); 
            GetComponent<PlayerBaseStats>().onLevelUp += RegenerateHealthAndMana;

            playerNameText.text = playerName;
        }


        private void Update() {
            CheckcurrentHealth();
            CheckCurrentMana();
            CheckCurrentExp();
            CheckCurrentStats();

            // GetComponent<PlayerBaseStats>().startingLevel = GetComponent<PlayerBaseStats>().GetLevel(); //update the level on the progression
           
        }

        public void TakeDamage(float damage)
        {
            damage -= armor;
            if(damage <= 0)
            {
                damage = 0;
            }
            if (damage > 0)
            {
                currentHealth = Mathf.Max(currentHealth - damage, 0);
                if (currentHealth <= 0)
                {
                    playerController.PlayerIsDead();
                }
            }
        }         

        private void CheckcurrentHealth()        
        {
            baseHealth = GetComponent<PlayerBaseStats>().GetStat(Stat.Health);
            maxHealth = baseHealth; //the max health is equal with baseHealth on a certain level + modifiers

            if (currentHealth >= maxHealth)
            {
                currentHealth = maxHealth;
                currentHealthBar.fillAmount = 1;
            }
            else
            {
                currentHealthBar.fillAmount = currentHealth / maxHealth;
            }
            currentHealthValue.text = currentHealth + " / " + maxHealth;
            if ((currentHealth <= 0))
            {
                currentHealth = 0;
                currentHealthBar.fillAmount = 0;
                DeadPlayer();
            }
        }

        private void CheckCurrentMana()
        {
            baseMana = GetComponent<PlayerBaseStats>().GetStat(Stat.Mana);
            maxMana = baseMana; //the max mana is equal with baseMana on a certain level + modifiers

            if (currentMana >= maxMana)
            {
                currentMana = maxMana;
                curentManaBar.fillAmount = 1;
            }
            else 
            {
                curentManaBar.fillAmount = currentMana / maxMana;
            }
            currentManaValue.text = currentMana + " / " + maxMana;
        }

        public void GainExperience(float experience)
        {
            currentExperience += (experience + playerStats.GetAdditiveModifiers(Stat.expToReward) + (experience * playerStats.GetPercentageModifiers(Stat.expToReward) / 100));
            Debug.Log("Exp rewarded --->>>>" +experience);
            // GetComponent<PlayerBaseStats>().CalculateLevel();
            onExperienceGained();
        }
        
        private void CheckCurrentExp()
        {
            // if (experience >= expNeededToLvl)
            // {
            //     baseMana = maxMana;
            //     curentManaBar.fillAmount = 1;
            // }
            // else
            // {
            //     curentManaBar.fillAmount = baseMana / maxMana;
            // }
            // baseManaValue.text = baseMana + " / " + maxMana;
            currentExpValue.text = "EXP: " +currentExperience;
        }

        private void CheckCurrentStats()
        {
            armor = GetComponent<PlayerBaseStats>().GetStat(Stat.armor);
        }
        
        private void DeadPlayer()
        {   
            fighter.dst += Time.deltaTime;
            
            Debug.Log("mort de "+ fighter.dst);
            if (fighter.dst >= fighter.deathTime)
            {
                //lose level or some procent from total exp
                currentExperience -= currentExperience * 20 / 100;

                // Respawn the player in the saved city
                GetComponent<PlayerBaseStats>().UpdateLevel();
                playerController.Respawn();

                currentHealth = maxHealth; // Restore the currentHealth to maxHealth

                // Attribute penality

                //drop a random item (equipped or from inventory) except normal potions;


                fighter.dst = 0; //reset the timer back to 0.
            }
        }

        private void RegenerateHealthAndMana()
        {
            currentHealth = GetComponent<PlayerBaseStats>().GetStat(Stat.Health); // set the currentHealth to the maximum value 
            currentMana = GetComponent<PlayerBaseStats>().GetStat(Stat.Mana);
        }

        public void RestoreHealth(float amountToRestore)
        {
            if(currentHealth > 0)
            {
                currentHealth += amountToRestore;
            }
        }

        public void RestoreMana(float amountToRestore)
        {
            currentMana += amountToRestore;
        }


        public object CaptureState()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["currentExperience"] = currentExperience;
            data["currentHealth"] = currentHealth;
            data["currentMana"] = currentMana;
            data["cooper"] = cooper;
            data["blueDiamonds"] = blueDiamonds;
            data["manaCrystals"] = manaCrystals;

            return data;
            // return currentHealth;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> data = (Dictionary<string, object>)state;
            currentExperience = (float)data["currentExperience"];
            currentHealth = (float)data["currentHealth"];
            currentMana = (float)data["currentMana"];
            cooper = (float)data["cooper"];
            blueDiamonds = (float)data["blueDiamonds"];
            manaCrystals = (float)data["manaCrystals"];


            // currentHealth = (float)state;
        }
    }
}