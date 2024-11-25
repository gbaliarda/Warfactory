using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillLevelCompletion : MonoBehaviour
{
    [SerializeField] private string _levelType;
    [SerializeField] private LevelTrain _levelTrain;

    private void OnDestroy()
    {
        _levelTrain.UnlockTrain();
    }
}
