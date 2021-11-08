using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    /// <summary>
    /// Each entry should contain a Quest and Objective.
    /// I have made this an outer class intentionally, to allow for easy creation of a PropertyDrawer
    /// which could be used to force the objectives to be an available objective within the Quest.
    /// </summary>
    [System.Serializable]
    public class CompletionObjective
    {

        public Quest quest;
        public string objective;
    }
    public class QuestCompletionMulti : MonoBehaviour
    {

        /// <summary>
        /// Each individual Quest/Objective should be in a separate record in this list.  I'm using List instead of
        /// an array to take advantage of Unity's improved list handling in the Editor.
        /// </summary>
        [SerializeField] private List<CompletionObjective> objectives = new List<CompletionObjective>();


        /// <summary>
        /// Attempts to complete an objective based on the index into the objectives list.  Your UnityEvent will show
        /// an int parameter when you select CompleteObject(int) from the drop down.  
        /// </summary>
        /// <param name="index"></param>
        public void CompleteObjective(int index)
        {
            QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            if (objectives.Count <= index || index < 0) return; //weed out bad indexes 
            //You should probably put error checking logic in QuestList.CompleteObjective() to ensure that the 
            //player has the quest.
            questList.CompleteObjective(objectives[index].quest, objectives[index].objective.ToString());
            Debug.Log($" quest-ul ESTE -->> {objectives[index].quest} <<--  si obiectivul este -->> {objectives[index].objective} <<-- ");
        } 
    }
}