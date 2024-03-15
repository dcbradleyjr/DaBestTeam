using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTrigger : MonoBehaviour
{
    [SerializeField] string finalSceneName;

    UnityEngine.SceneManagement.Scene currentScene;
    private void Awake()
    {
        currentScene = SceneManager.GetActiveScene();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        { 
            string sceneName = currentScene.name;
            int sceneIndex = currentScene.buildIndex;

            if (sceneName == finalSceneName)
            {
                gameManager.instance.youWin();
            }
            else
            {
                SceneManager.LoadScene(sceneIndex + 1);
            }
        }
    }
}
