using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Inventories;
using RPG.Saving;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour, ISaveable, IPredicateEvaluator
    {
        public List<QuestStatus> statuses = new List<QuestStatus>();
        
        public event Action onUpdate;

        public void AddQuest(Quest quest)
        {
            if (HasQuest(quest)) return;
            QuestStatus newStatus = new QuestStatus(quest);
            statuses.Insert(0, newStatus);

            if (onUpdate != null)
            {
                onUpdate();
            }
        }

        public void CompleteObjective(Quest quest, string objective)
        {
            QuestStatus status = GetQuestStatus(quest);
            if (status != null) status.CompleteObjective(objective);
            // if (status != null)
            // {
            //     if (!status.IsObjectiveComplete(objective))
            //     {
            //         status.CompleteObjective(objective);
            //     }
            // }

            if (onUpdate != null)
            {
                onUpdate();
            }
        }

        public bool HasQuest(Quest quest)
        {
            return GetQuestStatus(quest) != null;
        }

        public IEnumerable<QuestStatus> GetStatuses()
        {
            return statuses;
        }

        public QuestStatus GetQuestStatus(Quest quest)
        {
            foreach (QuestStatus status in statuses)
            {
                if (status.GetQuest() == quest)
                {
                    return status;
                }
            }
            return null;
        }

        // public void GiveReward(Quest quest)
        // {
        //     foreach (var reward in quest.GetRewards())
        //     {
        //         bool succes = GetComponent<Inventory>().AddToFirstEmptySlot(reward.item, reward.number);
        //         if (!succes)
        //         {
        //             GetComponent<ItemDropper>().DropItem(reward.item, reward.number);
        //         }
        //     }
        // }

        public void GiveReward()
        {
            foreach (var quest in statuses)
            {
                if (quest.IsComplete())
                {
                    foreach (var reward in quest.GetQuest().GetRewards())
                    {
                        bool succes = GetComponent<Inventory>().AddToFirstEmptySlot(reward.item, reward.number);
                        if (!succes)
                        {
                            GetComponent<ItemDropper>().DropItem(reward.item, reward.number);
                        }
                    }
                }
            }
        }

        public object CaptureState()
        {
            // Debug.Log("QuestList.CaptureState()");
            List<object> state = new List<object>();
            foreach (QuestStatus status in statuses)
            {
                // Debug.Log($"Adding {status.GetQuest().name}");
                state.Add(status.CaptureState());
            }
            return state;
        }

        public void RestoreState(object state)
        {
            // Debug.Log($"QuestList.RestoreState");
            List<object> stateList = state as List<object>;
            if (stateList == null) return;

            statuses.Clear();
            foreach (object objectState in stateList)
            {
                QuestStatus status = new QuestStatus(objectState);
                statuses.Add(status);
                // Debug.Log($"Restoring {status.GetQuest().name}");
            }

        }

        public bool? Evaluate(PredicateEnum predicate, string[] parameters)
        {
            switch (predicate)
            {
                case PredicateEnum.HasQuest:
                return HasQuest(Quest.GetByName(parameters[0]));

                case PredicateEnum.CompletedQuest:
                if(parameters[0] == null) return false;
                Quest questToTest = Quest.GetByName(parameters[0]);
                if (questToTest == null) return false;
                if (!HasQuest(questToTest)) return false;
                return GetQuestStatus(questToTest).IsComplete();
            }
            return null;
        }
    }

}