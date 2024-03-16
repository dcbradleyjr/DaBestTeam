using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FirstButtonSelect : MonoBehaviour
{
    private EventSystem m_EventSystem;

    public Button targetButton;

    void OnEnable()
    {
        // Fetch the current EventSystem. Make sure your Scene has one.
        m_EventSystem = EventSystem.current;

        // Get the local button if one wasn't set
        if (targetButton == null) targetButton = GetComponent<Button>();

        // Set it as active
        targetButton.Select();
    }

}
