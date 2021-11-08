using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestCompletion : MonoBehaviour
    {
        // [SerializeField] List<Quest> quest = new List<Quest>();
        // [SerializeField] List<string> objective = new List<string>();

        // public void CompleteObjective()
        // {
        //     QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();

        //     if (questList.statuses.Count() > 0)
        //     {
        //         foreach (Quest qst in quest)
        //         {
        //             if (questList.HasQuest(qst))
        //             {
        //                 foreach (var obj in objective)
        //                 {
        //                     if (qst.GetObjectives() != null)
        //                     {
        //                         foreach (var item in qst.GetObjectives())
        //                         {
        //                             if(item.reference == obj)
        //                             {
        //                                 questList.CompleteObjective(qst, obj);
        //                                 Debug.Log($"Obiectivul '{obj}' din quest-ul '{qst}' a fost completat");
        //                             }
        //                         }
        //                     }
        //                 }
        //             }
        //         }
        //     }
        // }

        [SerializeField] Quest quest;
        [SerializeField] string objective;

        public void CompleteObjective()
        {
            QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            questList.CompleteObjective(quest, objective);
            // questList.GetQuestStatus(quest).IsComplete();

        }

    }

}