using System.Collections;
using System.Collections.Generic;
using RPG.Objects;
using RPG.Inventories;
using UnityEngine;
using UnityEngine.Events;

public class TreasureChest : Interactable
{
    public bool isOpened = false;

    string action = "quest";
    [SerializeField] UnityEvent onTrigger;

    private void Trigger(string actionToTrigger)
    {
        if (actionToTrigger == action)
        {
            onTrigger.Invoke();
        }
    }

    public override void Interact()
    {
        if(!isOpened)
        {
            base.Interact();
            //Spawn items in the chest radius
            GetComponent<RandomDropper>().RandomDrop();
            //set the chest to being already opened
            isOpened = true;
            //change the tag
            this.tag = "Untagged";

            TriggerAction(action);
        }
    }

    private void TriggerAction(string action)
    {
        if (action == "") return;
        else
        {
            Trigger(action);
        }
    }
    

}
