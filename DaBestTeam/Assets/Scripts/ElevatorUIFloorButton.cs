using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ElevatorUI_FloorButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ButtonText;

    ElevatorController LinkedController;
    ElevatorFloor LinkedFloor;

    public void Bind(ElevatorFloor linkedFloor, ElevatorController linkedController, string floorName)
    {
        LinkedController = linkedController;
        LinkedFloor = linkedFloor;
        ButtonText.text = floorName;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPressed()
    {
        LinkedController.SendElevatorTo(LinkedFloor);
    }
}
