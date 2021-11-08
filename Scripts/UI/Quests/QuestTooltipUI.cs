using System.IO;
using System.Collections.Generic;
using RPG.Quests;
using TMPro;
using UnityEngine;
using System;
using System.Linq;
using RPG.Inventories;

namespace RPG.UI.Quests
{
    public class QuestTooltipUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] Transform objectiveContainer;
        [SerializeField] GameObject objectivePrefab;
        [SerializeField] GameObject objectiveIncompletePrefab;
        [SerializeField] TextMeshProUGUI rewardText;


        public void Setup(QuestStatus status)
        {
            Quest quest = status.GetQuest();
            
            title.text = quest.GetTitle();
            foreach (Transform item in objectiveContainer)
            {
                Destroy(item.gameObject);
            }

            foreach (var objective in quest.GetObjectives())
            {
                GameObject prefab = objectiveIncompletePrefab;
                if (status.IsObjectiveComplete(objective.reference))
                {
                    prefab = objectivePrefab;
                }
                
                GameObject objectiveInstance = Instantiate(prefab, objectiveContainer);
                TextMeshProUGUI objectiveText = objectiveInstance.GetComponentInChildren<TextMeshProUGUI>();
                objectiveText.text = objective.description;

                if (status.IsObjectiveComplete(objective.reference))
                {
                    objectiveText.color = Color.green;
                }

                if (objective.required >= 1)
                {
                    objectiveText.text += $" ({status.AmountCompleted(objective.reference)}/{objective.required})";
                }
            }
            rewardText.text = GetRewardText(quest);

        }

        private string GetRewardText(Quest quest)
        {
            string rewardText = "";
            foreach (var reward in quest.GetRewards())
            {
                if (rewardText != "")
                {
                    rewardText += ", ";
                }
                if (reward.number > 1)
                {
                    rewardText += reward.number + "x ";
                }
                rewardText += reward.item.GetDisplayName();
            }
            if (rewardText == "")
            {
                rewardText = "No reward";
            }
            rewardText += ".";
            return rewardText;
        }
    }

}