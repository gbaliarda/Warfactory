using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveDeleteBuildingMode : MonoBehaviour
{
    void Start()
    {
        EventManager.Instance.OnDeleteBuildModeActive += OnDeleteBuildModeActive;

        OnDeleteBuildModeActive(Player.Instance.DeleteBuildingMode);
    }

    public void OnDeleteBuildModeActive(bool active)
    {
        gameObject.SetActive(active);
    }

}
