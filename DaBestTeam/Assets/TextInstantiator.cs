using TMPro;
using UnityEngine;

public class TextInstantiator : MonoBehaviour
{
    [SerializeField] Transform canvas;
    [SerializeField] GameObject text;

    public void ScoreViewer(int value, string prompt, Color color)
    {
        GameObject score = GameObject.Instantiate(text,canvas);
        score.GetComponent<TextMeshProUGUI>().text = "+" + value.ToString() + " " + prompt;
        score.GetComponent<TextMeshProUGUI>().color = color;
    }

    public void Pickup(string prompt, Color color)
    {
        GameObject pickup = GameObject.Instantiate(text, canvas);
        pickup.GetComponent<TextMeshProUGUI>().text = prompt;
        pickup.GetComponent<TextMeshProUGUI>().color = color;
    }
}
