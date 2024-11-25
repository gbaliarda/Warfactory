using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ReturnTrainUI : Singleton<ReturnTrainUI>
{
    private CinemachineConfiner2D _cinemachineConfiner;

    void Start()
    {
        _cinemachineConfiner = GameObject.Find("VirtualCamera").GetComponent<CinemachineConfiner2D>();

        CloseReturnBase();
    }

    public void ReturnToBase()
    {
        Player.Instance.transform.SetPositionAndRotation(Base.Instance.Spawner.position, Player.Instance.transform.rotation);
        if (_cinemachineConfiner != null && Base.Instance.CameraConfiner != null)
        {
            _cinemachineConfiner.m_BoundingShape2D = Base.Instance.CameraConfiner;
            CinemachineVirtualCamera vcam = _cinemachineConfiner.GetComponent<CinemachineVirtualCamera>();
            if (vcam != null)
            {
                vcam.OnTargetObjectWarped(Player.Instance.transform, Base.Instance.Spawner.position - Player.Instance.transform.position);

                vcam.PreviousStateIsValid = false;
            }
        }

        Player.Instance.SetCurrentZone(GameObject.Find("Base"));
        Destroy(TemporalLevel.Instance.gameObject);
        CloseReturnBase();
    }

    public void CloseReturnBase()
    {
        gameObject.SetActive(false);
    }

    public void OpenReturnBase()
    {
        gameObject.SetActive(true);
    }
}
