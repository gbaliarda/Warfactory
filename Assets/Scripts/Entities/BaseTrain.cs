using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTrain : MonoBehaviour, IInteractable
{
    [SerializeField] private LevelPickerUI _levelPickerUI;

    public void Interact()
    {
        OpenMenu();
    }

    public void OpenMenu()
    {
        _levelPickerUI.OpenLevelPicker();
    }
}
