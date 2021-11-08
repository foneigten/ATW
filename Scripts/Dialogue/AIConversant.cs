using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Quests;
using UnityEngine;

namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour
    {
        [SerializeField] List<Dialogue> dialogues = new List<Dialogue>();
        [SerializeField] string conversantName;
        [SerializeField] bool chooseRandomDialogue;

        int currentDialogue = 0;

        void Awake()
        {
            this.tag = "Quest";

        }

        public void StartDialogue()
        {
            if (dialogues.Count == 0)
            {
                return;
            }

            if (dialogues.Count > 0)
            {
                if (chooseRandomDialogue)
                {
                    currentDialogue = Random.Range(0, dialogues.Count());
                }
                else
                {
                    currentDialogue = 0;
                }

                GameObject.FindWithTag("Player").GetComponent<PlayerConversant>().StartDialogue(this, dialogues[currentDialogue]);
            }

        }

        public void DeleteDialogFromNPC()
        {
            dialogues.Remove(dialogues[0]);
        }

        public void DeleteQuestFromNPC()
        {
            GetComponent<Quests.QuestGiver>().DeleteQuest();
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.CompareTag("Player"))
            {
                other.gameObject.GetComponent<PlayerConversant>().Quit();
            }
        }

        public string GetName()
        {
            return conversantName;
        }
    }
}
