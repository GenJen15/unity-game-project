using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;

public class ChangeInputManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Selectable firstInput;
    [SerializeField] private Button submitButton;

    private EventSystem eventSystem;

    private void Awake()
    {
        eventSystem = EventSystem.current;
        firstInput?.Select();
    }

    private void Update()
    {
        if (Keyboard.current == null) return; // No keyboard connected
        var keyboard = Keyboard.current;

        if (keyboard.tabKey.wasPressedThisFrame)
            NavigateTab(keyboard.leftShiftKey.isPressed || keyboard.rightShiftKey.isPressed);

        if ((keyboard.enterKey.wasPressedThisFrame || keyboard.numpadEnterKey.wasPressedThisFrame) && submitButton?.interactable == true)
            submitButton.onClick.Invoke();
    }

    private void NavigateTab(bool shift)
    {
        var current = eventSystem.currentSelectedGameObject?.GetComponent<Selectable>() ?? firstInput;
        if (current == null) return;

        Selectable next = shift
            ? current.FindSelectableOnUp() ?? GetLastSelectable()
            : current.FindSelectableOnDown() ?? firstInput;

        next?.Select();
    }

    private Selectable GetLastSelectable()
    {
        if (firstInput == null) return null;
        var current = firstInput;
        while (current.FindSelectableOnDown() != null)
            current = current.FindSelectableOnDown();
        return current;
    }
}