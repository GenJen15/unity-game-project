using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ChangeInput : MonoBehaviour
{
    EventSystem eventSystem;
    public Selectable firstInput;
    public Button submitButton;

    void Start()
    {
        eventSystem = EventSystem.current;
    }

    void Update()
    {
        // TAB or SHIFT+TAB navigation
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (eventSystem.currentSelectedGameObject == null)
            {
                firstInput.Select();
                return;
            }

            var current = eventSystem.currentSelectedGameObject.GetComponent<Selectable>();
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                var previous = current.FindSelectableOnUp();
                if (previous != null)
                    previous.Select();
            }
            else
            {
                var next = current.FindSelectableOnDown();
                if (next != null)
                    next.Select();
            }
        }

        // ENTER / RETURN to submit
        if (Input.GetKeyDown(KeyCode.Return))
        {
                submitButton.onClick.Invoke();
                Debug.Log("Submit button clicked via keyboard");
        }
    }
}