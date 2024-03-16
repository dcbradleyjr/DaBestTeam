using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    [SerializeField] GameObject loadingScreen;
  

    [SerializeField] Slider loadingSlider;

    public void LoadLevelButton(string level)
    {
        loadingScreen.SetActive(true);

        // run async
        StartCoroutine(LoadLevelASync(level));
    }

    IEnumerator LoadLevelASync(string level)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(level);

        while (!op.isDone)
        {
            float progressValue = Mathf.Clamp01(op.progress / 0.9f);
            loadingSlider.value = progressValue;
            yield return new WaitForSeconds(5f);
        }
        yield return new WaitForSeconds(5f);
    }
}