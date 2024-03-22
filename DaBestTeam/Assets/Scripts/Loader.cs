using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    [SerializeField] Slider loadingSlider;
    [SerializeField] GameObject deactivatedScreen;
    [SerializeField] GameObject loadedScreen;
    public bool isLoading;

    public void LoadLevelButton()
    {
        if (deactivatedScreen)
        deactivatedScreen.SetActive(false);
        loadedScreen.SetActive(true);
        isLoading = true;

        Debug.Log("Trying to load");

        if(PlayerPrefs.HasKey("SavedSceneIndex"))
        {
            int index = PlayerPrefs.GetInt("SavedSceneIndex", 1);
            StartCoroutine(FakeLoadLevelASync(index));
        }
        else
        StartCoroutine(FakeLoadLevelASync(1));
    }
    IEnumerator FakeLoadLevelASync(int index)
    {
        Debug.Log("loading");
        float elapsedTime = 0f;
        float duration = 1f; // 3 seconds
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progressValue = Mathf.Clamp01(elapsedTime / duration);
            loadingSlider.value = progressValue;
            yield return null; // Wait for the next frame
        }
        AsyncOperation op = SceneManager.LoadSceneAsync(index);

        while (!op.isDone)
        {
            float progressValue = Mathf.Clamp01(op.progress / 0.9f);
            loadingSlider.value = progressValue;
            yield return null; // Wait for the next frame
        }
    }
    IEnumerator LoadLevelASync(int index)
    {

        AsyncOperation op = SceneManager.LoadSceneAsync(index);

        while (!op.isDone)
        {
            float progressValue = Mathf.Clamp01(op.progress / 0.9f);
            loadingSlider.value = progressValue;
            yield return new WaitForSecondsRealtime(5f);
        }

    }
}