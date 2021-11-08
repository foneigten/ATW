using System.Collections;
using System.Collections.Generic;
using RPG.Objects;
using UnityEngine;
using UnityEngine.UI;

public class Sign : Interactable
{
    public GameObject dialogBox;
    public Text dialogText;
    public string dialog;
    
    
    // Update is called once per frame
    void Update()
    {
        // if(playerInRange)
        // {
        //     GetComponent<RPG.Control.PlayerController>().CanInteract();
        // }

        if(playerInRange)
        {
            if(!dialogBox.activeInHierarchy)
            {
                dialogBox.SetActive(true);
                dialogText.text = dialog;
            }
            else
            {
                dialogBox.SetActive(false);
            }
        }

    }
}
