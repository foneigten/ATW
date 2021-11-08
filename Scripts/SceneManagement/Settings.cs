using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Settings : MonoBehaviour
    {
        const string defaultSaveFile = "ATW_save";
        // float timePassed = 0;
        // float timeBeforeSaving = 60;

        [SerializeField] GameObject saveMenu;
       

        // private void Update() {
        //     timePassed += Time.deltaTime;
        //     if (timePassed >= timeBeforeSaving)
        //     {
        //         timePassed = 0;
        //         GetComponent<SavingSystem>().Save("" + GetComponent<Core.PlayerStats>().playerName + "_AutoSave");
        //         // GetComponent<SavingSystem>().Save(defaultSaveFile);
        //     }            
        // }

        public void Options()
        {
            if (!saveMenu.activeInHierarchy)
            {
                saveMenu.SetActive(true);
            }
            else if (saveMenu.activeInHierarchy)
            {
                saveMenu.SetActive(false);
            }
        }



        public void Save()
        {
            FindObjectOfType<SavingSystem>().Save(defaultSaveFile); //saving the game manually with the player`s name ==> will be done automatic every frame.
        }

        public void Load()
        {
            // FindObjectOfType<SavingSystem>().Load(defaultSaveFile); //loading the game manually ==> will be loaded on character choose.
            // FindObjectOfType<SavingSystem>().LoadLastScene(defaultSaveFile);
            FindObjectOfType<SavingSystem>().StartCoroutine("LoadLastScene");
        }

        public void Delete()
        {
            FindObjectOfType<SavingSystem>().Delete(defaultSaveFile); //delete manually the last manually saved game ==> will be deleted on deleting the character
        }

        public void ExitGame()
        {
            Application.Quit();
        }

    }
}
