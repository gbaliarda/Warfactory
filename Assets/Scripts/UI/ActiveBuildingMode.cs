using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBuildingMode : MonoBehaviour
{
    void Start()
    {
        EventManager.Instance.OnBuildModeActive += OnBuildModeActive;

        OnBuildModeActive(Player.Instance.BuildingMode);
    }

    public void OnBuildModeActive(bool active)
    {
        gameObject.SetActive(active);
    }

}
