using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    [SerializeField] Slider loadingSlider;
    public bool isLoading;

    public void LoadLevelButton(string level)
    {
        UIManager.instance.loadingScreen.SetActive(true);
        UIManager.instance.MainMenu.SetActive(false);
        isLoading = true;

        // run async
        StartCoroutine(LoadLevelASync(level));
    }

    IEnumerator LoadLevelASync(string level)
    {
        yield return new WaitForSeconds(5f);
        AsyncOperation op = SceneManager.LoadSceneAsync(level);

        while (!op.isDone)
        {
            float progressValue = Mathf.Clamp01(op.progress / 0.9f);
            loadingSlider.value = progressValue;
            yield return new WaitForSeconds(5f);
        }
    }
}