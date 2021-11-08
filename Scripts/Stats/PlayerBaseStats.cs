using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace RPG.Stats
{

    public class PlayerBaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] public int startingLevel = 1;
        [SerializeField] PlayerClass playerClass;
        [SerializeField] PlayerProgression playerProgression = null;
        // [SerializeField] GameObject levelUpParticleEffect = null;

        public event Action onLevelUp;

        int currentLevel = 0;

        private void Start() {
            currentLevel = CalculateLevel();

            GetComponent<PlayerStats>().onExperienceGained += UpdateLevel;
        }

        private void OnEnable() {
            GetComponent<PlayerStats>().onExperienceGained += UpdateLevel;
            Debug.Log("The PlayerBaseStats is enabled");
        }

        private void OnDisable() {
            GetComponent<PlayerStats>().onExperienceGained -= UpdateLevel;
            Debug.Log("The PlayerBaseStats is disabled");
        }

        public void UpdateLevel() {
            int newLevel = CalculateLevel();

            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
                // LevelUpEffect();
                print("You have leveled up.");
                onLevelUp();
            }
            if (newLevel < currentLevel)
            {
                currentLevel = newLevel;
                print("You just got leveled down");
            }
        }

        // private void LevelUpEffect()
        // {
        //     Instantiate(levelUpParticleEffect, transform);
        // }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifiers(stat)) * (1 + GetPercentageModifiers(stat) / 100);
        }

        
        private float GetBaseStat(Stat stat)
        {
            return playerProgression.GetStat(stat, playerClass, GetLevel());
        }

        public int GetLevel()
        {
            if(currentLevel < 1)
            {
                currentLevel = CalculateLevel();
            }
            return currentLevel;
        }

        private int CalculateLevel()
        {
            if (GetComponent<PlayerStats>().currentExperience < 1) return startingLevel;

            float currentXP = GetComponent<PlayerStats>().currentExperience;
            int penultimateLevel = playerProgression.GetLevels(Stat.ExperienceToLevelUp, playerClass);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float XPToLevelUp = playerProgression.GetStat(Stat.ExperienceToLevelUp, playerClass, level);
                if (XPToLevelUp > currentXP)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }

        public float GetAdditiveModifiers(Stat stat)
        {
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        public float GetPercentageModifiers(Stat stat)
        {
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }


    }
}