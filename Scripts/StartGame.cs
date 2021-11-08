using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    int sceneToLoad = 1;

    public void PlayTheGame()
    {
        SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
