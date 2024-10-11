using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class BasePortal : MonoBehaviour
{
    private CinemachineConfiner2D _cinemachineConfiner;


    private static BasePortal instance;
    public static BasePortal Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && this.gameObject != null)
        {
            BasePortal oldInstance = instance;
            instance = this;
            Destroy(oldInstance.gameObject);
            return;
        }
        instance = this;
    }

    void Start()
    {
        _cinemachineConfiner = GameObject.Find("VirtualCamera").GetComponent<CinemachineConfiner2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponentInParent<Player>() != null)
        {
            other.transform.parent.SetPositionAndRotation(Base.Instance.Spawner.position, other.transform.parent.rotation);
            if (_cinemachineConfiner != null && Base.Instance.CameraConfiner != null)
            {
                _cinemachineConfiner.m_BoundingShape2D = Base.Instance.CameraConfiner;
                CinemachineVirtualCamera vcam = _cinemachineConfiner.GetComponent<CinemachineVirtualCamera>();
                if (vcam != null)
                {
                    vcam.OnTargetObjectWarped(other.transform, Base.Instance.Spawner.position - other.transform.position);

                    vcam.PreviousStateIsValid = false;
                }
            }

            Destroy(TemporalLevel.Instance.gameObject);
            Destroy(LevelPortal.Instance.gameObject);
        }
    }
}
