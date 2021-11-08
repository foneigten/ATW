using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        public enum ActionEnum
        {
            None,
            GiveQuest,
            DeleteQuest,
            CompleteQuest,
            CompleteObjective
        }

        [SerializeField] ActionEnum action;
        [SerializeField] UnityEvent onTrigger;

        public void Trigger(ActionEnum actionToTrigger)
        {
            
            if (actionToTrigger == action)
            {
                onTrigger.Invoke();
            }
        }
    }
}
