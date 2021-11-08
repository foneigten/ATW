using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class ShowHideUI : MonoBehaviour
    {
        // [SerializeField] KeyCode toggleKey = KeyCode.Escape;
        [SerializeField] GameObject inventoryWindow = null;
        [SerializeField] GameObject equipmentWindow = null;
        [SerializeField] GameObject questWindow = null;
        [SerializeField] Scrollbar invScrollbar = null;


        void Start()
        {
            inventoryWindow.SetActive(false);
            equipmentWindow.SetActive(false);
            questWindow.SetActive(false);
        }


        public void ActivateInventoryWindow()
        {
            inventoryWindow.SetActive(!inventoryWindow.activeSelf);
            invScrollbar.value = 1;
        }

        public void ActivateEquipmentWindow()
        {
            equipmentWindow.SetActive(!equipmentWindow.activeSelf);
        }

        public void ActivateQuestWindow()
        {
            questWindow.SetActive(!questWindow.activeSelf);
        }
    }
}