using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FirstButtonSelect : MonoBehaviour
{
    private EventSystem m_EventSystem;
    private Button targetButton;

    void OnEnable()
    {
        // Fetch the current EventSystem. Make sure your Scene has one.
        m_EventSystem = EventSystem.current;

        // Get the button component attached to this GameObject
        targetButton = GetComponent<Button>();

        SelectInitialButton();        
    }

    void Update()
    {
        if (m_EventSystem.currentSelectedGameObject == null)
            targetButton.Select();
    }

    void SelectInitialButton()
    {
        // Set it as active
        targetButton.Select();       
    }

    IEnumerator TriggerPointerEnter()
    {
        yield return null;

        // Trigger pointer enter event
        ExecuteEvents.Execute(targetButton.gameObject, new PointerEventData(m_EventSystem), ExecuteEvents.pointerMoveHandler);
    }
}
