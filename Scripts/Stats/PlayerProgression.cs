using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "PlayerProgression", menuName = "Stats/New Progression", order = 0)]
    public class PlayerProgression : ScriptableObject
    {
        [SerializeField] ProgressionPlayerClass[] playerClasses = null;

        Dictionary<PlayerClass, Dictionary<Stat, float[]>> lookupTable = null;

        public float GetStat(Stat stat, PlayerClass playerClass, int level)
        {
            BuildLookup();

            float[] levels = lookupTable[playerClass][stat];

            if (levels.Length < level)
            {
                return 0;
            }

            return levels[level - 1];
        }

        public int GetLevels(Stat stat, PlayerClass playerClass)
        {
            BuildLookup();

            float[] levels = lookupTable[playerClass][stat];
            return levels.Length;
        }

        private void BuildLookup()
        {
            if (lookupTable != null) return;

            lookupTable = new Dictionary<PlayerClass, Dictionary<Stat, float[]>>();

            foreach (ProgressionPlayerClass progressionClass in playerClasses)
            {
                var statLookupTable = new Dictionary<Stat, float[]>();

                foreach (ProgressionStat progressionStat in progressionClass.stats)
                {
                    statLookupTable[progressionStat.stat] = progressionStat.levels;
                }

                lookupTable[progressionClass.playerClass] = statLookupTable;
            }
        }

        [System.Serializable]
        class ProgressionPlayerClass
        {
            public PlayerClass playerClass;
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }
    }
}