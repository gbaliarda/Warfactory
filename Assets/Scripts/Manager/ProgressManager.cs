using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ProgressManager : Singleton<ProgressManager>
{
    [SerializeField]
    private int _offenseProgress;
    public int OffenseProgress
    {
        get => Mathf.Clamp(_offenseProgress, 0, 5);
        set
        {
            _offenseProgress = value;
            OnProgressChange?.Invoke();
        }
    }

    [SerializeField]
    private int _defenseProgress;
    public int DefenseProgress
    {
        get => Mathf.Clamp(_defenseProgress, 0, 5);
        set
        {
            _defenseProgress = value;
            OnProgressChange?.Invoke();
        }
    }

    [SerializeField]
    private UnityAction _onProgressChange;
    public UnityAction OnProgressChange { get => _onProgressChange; set => _onProgressChange = value; }

    public void LevelCompleted(LevelType levelType, int difficulty)
    {
        var newStars = difficulty + 1;

        switch (levelType)
        {
            case LevelType.Offense:
                OffenseProgress = Mathf.Max(OffenseProgress, newStars);
                break;
            case LevelType.Defense:
                DefenseProgress = Mathf.Max(DefenseProgress, newStars);
                break;
        }
    }

    private void OnValidate()
    {
        OnProgressChange?.Invoke();
    }
}