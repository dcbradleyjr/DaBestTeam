using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class InteractionPromptUI : MonoBehaviour
{
    [SerializeField] GameObject UIPanel;
    [SerializeField] TextMeshProUGUI m_TextMeshProUGUI;

    public bool IsDisplayed = false;

    private void Start()
    {
        UIPanel.SetActive(false);
    }

    public void SetUp(string promptText)
    {
        m_TextMeshProUGUI.text = promptText;
        UIPanel.SetActive(true);
        IsDisplayed = true;
    }

    public void Close()
    {
        UIPanel.SetActive(false);
        IsDisplayed = false;
    }

}
