using System.Collections;
using RPG.Saving;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        // Vector3 respawnPoint = new Vector3(-1, -2, 0);

        // private void Awake() 
        // {
        //     StartCoroutine(LoadLastScene());
        // }

        // IEnumerator LoadLastScene() 
        // {
        //     respawnPoint = GetComponent<Core.PlayerStats>().defaultSpawnCity;

        //     yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
        //     if(GameObject.FindWithTag("Player").GetComponent<Transform>().position != respawnPoint)
        //     {
        //         yield return GameObject.FindWithTag("Player").GetComponent<Transform>().position = respawnPoint;
        //     }
        // Fader fader = FindObjectOfType<Fader>();

        // fader.FadeOut(0.01f);
        //     yield return fader.FadeIn(0.2f);
        // }


    }
}