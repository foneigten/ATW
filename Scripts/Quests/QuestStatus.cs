using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestStatus
    {
        Quest quest;
        Dictionary<string, int> completedObjectives = new Dictionary<string, int>();
        public bool questAlreadyCompleted = false;

        [System.Serializable]
        class QuestStatusRecord
        {
            public string questName;
            public Dictionary<string, int> completedObjectives = new Dictionary<string, int>();
        }

        public QuestStatus(Quest quest)
        {
            this.quest = quest;
        }

        public QuestStatus(object objectState)
        {
            QuestStatusRecord state = objectState as QuestStatusRecord;
            quest = Quest.GetByName(state.questName);

            // if(IsObjectiveComplete(quest.GetObjectives().ToString()))
            // {
            //     completedObjectives = state.completedObjectives;
            // }
            completedObjectives = state.completedObjectives;
        }

        public Quest GetQuest()
        {
            return quest;
        }

        public bool IsComplete()
        {
            foreach (var objective in quest.GetObjectives())
            {
                Debug.Log($" Trebuie testat daca este adevarat  --->>>>   {completedObjectives.ContainsKey(objective.reference)}");
                Debug.Log($" Comparam daca   {AmountCompleted(objective.reference)}   este mai mic decat {objective.required}  ");
                Debug.Log($"Numarul de obiective completate din quest-ul {objective.reference}  sunt  {AmountCompleted(objective.reference)}  si trebuie {objective.required}");
                if (!completedObjectives.ContainsKey(objective.reference) || AmountCompleted(objective.reference) < objective.required)
                // if (!completedObjectives.ContainsKey(objective.reference) || completedObjectives[objective.reference] < objective.required)
                {
                    Debug.Log($"Objective {objective} este incomplet");
                    return false;
                }
                Debug.Log($" Objective -> {objective} <- este complet.");
            }
            Debug.Log($"Quest-ul este COMPLETAT");
            questAlreadyCompleted = true;
            return true;
        }

        public int GetCompletedCount()
        {
            int completedObjectives = 0;
            foreach (var obj in quest.GetObjectives())
            {
                Debug.Log($"Questul este  {quest.name}  si Obiectivul verificat este {obj.reference}  si trebuie facut de   {obj.required}  ori");
                if (IsObjectiveComplete(obj.reference))
                {
                    completedObjectives += 1;
                }
            }
            Debug.Log($"numarul obiectivelor complete este  {completedObjectives}");
            return completedObjectives;
        }

        public bool IsObjectiveComplete(string objective)
        {
            // Quest.Objective testObjective = quest.GetObjective(objective);

            // if (!completedObjectives.ContainsKey(objective)) return false; // check if the objective is done (at least once)
            // else 
            // {
            //     if (!completedObjectives.ContainsValue(testObjective.required)) return false; // check if the objective is completed as many times as we need it to be
            // }
            // return (testObjective != null) && AmountCompleted(objective) >= testObjective.required;
            
            if (!completedObjectives.ContainsKey(objective)) return false;
            Quest.Objective testObjective = quest.GetObjective(objective);
            return (testObjective != null) && completedObjectives[objective] >= testObjective.required;
        }


        public void CompleteObjective(string objective)
        {
            if (quest.HasObjective(objective) && !IsObjectiveComplete(objective))
            {
                if (!completedObjectives.ContainsKey(objective))
                {
                    completedObjectives[objective] = 0; //creates a new entry
                }
                completedObjectives[objective] += 1;
            }
        }

        public int AmountCompleted(string objective)
        {
            if (completedObjectives.ContainsKey(objective))
            {
                return completedObjectives[objective];
            }
            return 0;
        }


        public object CaptureState()
        {
            QuestStatusRecord state = new QuestStatusRecord();
            state.questName = quest.name;
            state.completedObjectives = completedObjectives;

            return state;
        }
    }
}
