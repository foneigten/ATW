using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        PlayerBaseStats playerBaseStats;

        private void Awake() {
            playerBaseStats = GameObject.FindWithTag("Player").GetComponent<PlayerBaseStats>();
        }
        
        private void Update() {
            GetComponent<Text>().text = String.Format("Level: {0:0}", playerBaseStats.GetLevel());
        }
    }

}