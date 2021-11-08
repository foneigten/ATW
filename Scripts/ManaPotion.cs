using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Inventories;
using UnityEngine;

[CreateAssetMenu(fileName = "ManaPotion", menuName = "RPG/Inventory/Mana Potion")]
public class ManaPotion : ActionItem
{
    [SerializeField] float amountToRestore;

    public override void Use(GameObject user)
    {
        user.GetComponent<PlayerStats>().RestoreMana(amountToRestore);
    }
}