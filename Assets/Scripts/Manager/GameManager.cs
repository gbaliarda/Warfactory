using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private TemporalLevel _introLevel;
    private CinemachineConfiner2D _cinemachineConfiner;
    private bool _tutorialCompleted = false;

    public bool TutorialCompleted => _tutorialCompleted;

    private void Start()
    {
        _cinemachineConfiner = GameObject.Find("VirtualCamera").GetComponent<CinemachineConfiner2D>();

        if (!_tutorialCompleted)
            InstantiateTutorial();
    }

    public void CompleteTutorial()
    {
        _tutorialCompleted = true;
    }

    public void InstantiateTutorial()
    {
        Instantiate(_introLevel, GameObject.Find("LevelZone").transform);
        TeleportToTutorial();
    }

    public void TeleportToTutorial()
    {
        Player.Instance.SetCurrentZone(TemporalLevel.Instance.gameObject);
        TemporalLevel.Instance.UpdateTileManager();

        Player.Instance.transform.SetPositionAndRotation(TemporalLevel.Instance.Spawner.position, Player.Instance.transform.rotation);
        if (_cinemachineConfiner != null && TemporalLevel.Instance.CameraConfiner != null)
        {
            _cinemachineConfiner.m_BoundingShape2D = TemporalLevel.Instance.CameraConfiner;
            CinemachineVirtualCamera vcam = _cinemachineConfiner.GetComponent<CinemachineVirtualCamera>();
            if (vcam != null)
            {
                vcam.OnTargetObjectWarped(Player.Instance.transform, TemporalLevel.Instance.Spawner.transform.position - Player.Instance.transform.position);

                vcam.PreviousStateIsValid = false;
            }
        }
    }
}
