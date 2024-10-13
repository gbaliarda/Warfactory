using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Collider2D))]
public class LevelPortal : MonoBehaviour
{
    [SerializeField] private TemporalLevel _temporalLevelToInstantiate;
    private CinemachineConfiner2D _cinemachineConfiner;

    private static LevelPortal instance;
    public static LevelPortal Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && this.gameObject != null)
        {
            LevelPortal oldInstance = instance;
            instance = this;
            Destroy(oldInstance.gameObject);
            Debug.Log("Level destroyed");
            return;
        }
        instance = this;
    }

    void Start()
    {
        Debug.Log("Starting Level");
        _cinemachineConfiner = GameObject.Find("VirtualCamera").GetComponent<CinemachineConfiner2D>();

        Instantiate(_temporalLevelToInstantiate, GameObject.Find("LevelZone").transform);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponentInParent<Player>() != null)
        {
            Player.Instance.SetCurrentZone(TemporalLevel.Instance.gameObject);
            other.transform.parent.SetPositionAndRotation(TemporalLevel.Instance.Spawner.position, other.transform.parent.rotation);
            if (_cinemachineConfiner != null && TemporalLevel.Instance.CameraConfiner != null)
            {
                _cinemachineConfiner.m_BoundingShape2D = TemporalLevel.Instance.CameraConfiner;
                CinemachineVirtualCamera vcam = _cinemachineConfiner.GetComponent<CinemachineVirtualCamera>();
                if (vcam != null)
                {
                    vcam.OnTargetObjectWarped(other.transform, TemporalLevel.Instance.Spawner.transform.position - other.transform.position);

                    vcam.PreviousStateIsValid = false;
                }
            }
        }
    }
}
