using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Inventories;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthPotion", menuName = "RPG/Inventory/Health Potion")]
public class HealthPotion : ActionItem
{
    [SerializeField] float amountToRestore;

    public override void Use(GameObject user)
    {
        user.GetComponent<PlayerStats>().RestoreHealth(amountToRestore);
    }
}