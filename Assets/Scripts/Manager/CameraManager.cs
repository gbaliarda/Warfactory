using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : Singleton<CameraManager>
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    public void SetPlayerCameraFollow()
    {
        cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        cinemachineVirtualCamera.Follow = Player.Instance.transform;
    }
}
