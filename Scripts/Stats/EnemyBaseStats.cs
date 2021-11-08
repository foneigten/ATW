using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class EnemyBaseStats : MonoBehaviour
    {
        [Range(1, 120)]
        public int enemyLevel = 1;
        [SerializeField] EnemyClass enemyClass;
        [SerializeField] EnemyProgression enemyProgression = null;

        public float GetStat(Stat stat)
        {
            return enemyProgression.GetStat(stat, enemyClass, enemyLevel);
        }
    }
}