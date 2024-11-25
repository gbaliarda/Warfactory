using UnityEngine;
using System.Collections;

public class TimerLevelCompletion : MonoBehaviour
{
    [SerializeField] private float startTimer = 60f;
    [SerializeField] private float completeTimer = 600f;
    [SerializeField] private EnemySpawners _spawners;
    [SerializeField] private LevelTrain _levelTrain;
    [SerializeField] private GameObject _trainBlock;

    private bool startEventTriggered = false;
    private bool completeEventTriggered = false;

    private float elapsedTime = 0f;

    void Start()
    {
        elapsedTime = 0f;
        _spawners.InactiveAllSpawners();

        TimerUI.Instance.ShowTimer();
        TimerUI.Instance.SetTime(startTimer);
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime < startTimer)
        {
            TimerUI.Instance.SetTime(startTimer - elapsedTime);
        }
        else if (elapsedTime < completeTimer)
        {
            TimerUI.Instance.SetTime(completeTimer - elapsedTime);
        } else
        {
            TimerUI.Instance.SetTime(0);
            TimerUI.Instance.HideTimer();
        }

        if (!startEventTriggered && elapsedTime >= startTimer)
        {
            startEventTriggered = true;
            OnStartTimer();
        }

        if (!completeEventTriggered && elapsedTime >= completeTimer)
        {
            completeEventTriggered = true;
            OnCompleteTimer();
        }
    }

    private void OnStartTimer()
    {
        _spawners.ActiveAllSpawners();
    }

    private void OnCompleteTimer()
    {
        _spawners.InactiveAllSpawners();
        _levelTrain.UnlockTrain();
        _trainBlock.SetActive(false);
    }
}
