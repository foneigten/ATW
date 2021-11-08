using System;
using System.Collections;
using RPG.Saving;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D, E, F, G, H
        }

        [Header("Portal Variables")]
        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [Tooltip("Make sure that this & corresponding spawnpoint have same Destination")]
        [SerializeField] DestinationIdentifier destination;

        [Header("Fader configuration")]
        [SerializeField] float fadeOutTime = 0.3f;
        [SerializeField] float fadeInTime = 0.4f;
        [SerializeField] float fadeWaitTime = 3f;

        void OnTriggerEnter2D(Collider2D other) {
            if(other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError("Scene to load is not set.");
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();
            // yield return fader.FadeOut(fadeOutTime);
            yield return fader.FadeRoutine(1, fadeOutTime);
            
            FindObjectOfType<SavingSystem>().Save("Auto_Save");

            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            FindObjectOfType<SavingSystem>().Load("Auto_Save");

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            FindObjectOfType<SavingSystem>().Save("Auto_Save");

            yield return new WaitForSeconds(fadeWaitTime);
            // yield return fader.FadeIn(fadeInTime);
            yield return fader.FadeRoutine(0, fadeInTime);

            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player =  GameObject.FindWithTag("Player");
            player.transform.position = otherPortal.spawnPoint.transform.position;

        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                
                if (portal.destination != destination) continue;

                return portal;
                
            }
            return null;
        }
    }
}
