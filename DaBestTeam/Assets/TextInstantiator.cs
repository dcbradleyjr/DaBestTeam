using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class TextInstantiator : MonoBehaviour
{
    [SerializeField] Transform canvas;
    [SerializeField] GameObject text;

    public void ScoreViewer(int value, string prompt)
    {
        GameObject score = GameObject.Instantiate(text,canvas);
        score.GetComponent<TextMeshProUGUI>().text = "+" + value.ToString() + " " + prompt;
    }

    public void Pickup(string prompt, Color color)
    {
        GameObject pickup = GameObject.Instantiate(text, canvas);
        pickup.GetComponent<TextMeshProUGUI>().text = prompt;
        pickup.GetComponent<TextMeshProUGUI>().color = color;
    }
}
