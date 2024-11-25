using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class LevelTrain : MonoBehaviour, IInteractable
{
    [SerializeField] private string _currentLevel;

    public void Interact()
    {
        OpenMenu();
    }

    public void OpenMenu()
    {
        ReturnTrainUI.Instance.OpenReturnBase();
        if (_currentLevel == "Offense")
        {
            LevelPickerUI.Instance.UnlockDefenseLevel();
        } else if (_currentLevel == "Intro")
        {
            GameManager.Instance.CompleteTutorial();
        }
    }

    public void UnlockTrain()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
