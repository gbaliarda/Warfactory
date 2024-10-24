using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteBuildingButton : MonoBehaviour
{
    void Start()
    {
        EventManager.Instance.OnBuildModeActive += OnBuildModeActive;

        gameObject.SetActive(Player.Instance.BuildingMode);       
    }


    public void OnBuildModeActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
