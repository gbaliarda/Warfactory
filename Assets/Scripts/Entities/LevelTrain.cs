using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class LevelTrain : MonoBehaviour
{
    [SerializeField] private string _currentLevel;
    public void OpenMenu()
    {
        ReturnTrainUI.Instance.OpenReturnBase();
        if (_currentLevel == "Offense")
        {
            LevelPickerUI.Instance.UnlockDefenseLevel();
        }
    }

    public void UnlockTrain()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
