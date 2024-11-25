using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTrain : MonoBehaviour
{
    [SerializeField] private LevelPickerUI _levelPickerUI;
    public void OpenMenu()
    {
        _levelPickerUI.OpenLevelPicker();
    }
}
