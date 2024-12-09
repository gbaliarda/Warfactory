using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class LevelTrain : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        OpenMenu();
    }

    public void OpenMenu()
    {
        ReturnTrainUI.Instance.OpenReturnBase();
        TemporalLevel.Instance.CompleteLevel();
    }

    public void UnlockTrain()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
