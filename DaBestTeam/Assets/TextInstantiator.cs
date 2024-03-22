using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextInstantiator : MonoBehaviour
{
    [SerializeField] Transform canvas;
    [SerializeField] GameObject text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ScoreViewer(int value)
    {
        GameObject score = GameObject.Instantiate(text,canvas);
        score.GetComponent<TextMeshProUGUI>().text = "+ " + value.ToString() + " earned";
    }
}
