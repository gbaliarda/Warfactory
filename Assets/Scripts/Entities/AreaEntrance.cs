using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    [SerializeField] private string _transitionName;

    private void Start()
    {
        if (_transitionName == EldritchSceneManager.Instance.SceneTransitionName)
        {
            Player.Instance.transform.position = this.transform.position;
            CameraManager.Instance.SetPlayerCameraFollow();
        }
    }
}
