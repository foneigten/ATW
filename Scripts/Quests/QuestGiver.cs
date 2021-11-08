using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestGiver : MonoBehaviour
    {
        [SerializeField] List<Quest> quests = new List<Quest>();
        // [SerializeField] Quest[] quests;

        public void GiveQuest(Quest quest)
        {
            QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            // questList.AddQuest(quests[0]);
            questList.AddQuest(quests[0]);
            
            DeleteQuest();
        }

        public void DeleteQuest()
        {
            quests.Remove(quests[0]);
        }

    }

}