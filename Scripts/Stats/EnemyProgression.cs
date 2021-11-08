using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "EnemyProgression", menuName = "Stats/Enemy Progression", order = 1)]
    public class EnemyProgression : ScriptableObject
    {
        [SerializeField] ProgressionEnemyClass[] enemyClasses = null;

        Dictionary<EnemyClass, Dictionary<Stat, float[]>> lookupTable = null;

        public float GetStat(Stat stat, EnemyClass enemyClass, int level)
        {
            BuildLookup();

            float[] levels = lookupTable[enemyClass][stat];

            if (levels.Length < level)
            {
                return 0;
            }

            return levels[level - 1];
        }

        private void BuildLookup()
        {
            if (lookupTable != null) return;

            lookupTable = new Dictionary<EnemyClass, Dictionary<Stat, float[]>>();

            foreach (ProgressionEnemyClass progressionClass in enemyClasses)
            {
                var statLookupTable = new Dictionary<Stat, float[]>();

                foreach (ProgressionStat progressionStat in progressionClass.stats)
                {
                    statLookupTable[progressionStat.stat] = progressionStat.levels;
                }

                lookupTable[progressionClass.enemyClass] = statLookupTable;
            }
        }

        [System.Serializable]
        class ProgressionEnemyClass
        {
            public EnemyClass enemyClass;
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