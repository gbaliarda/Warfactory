using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreenUI : Singleton<DeathScreenUI>
{
    void Start()
    {
        gameObject.SetActive(false);        
    }

    public void Popup()
    {
        gameObject.SetActive(true);
    }

    public void RespawnAtBase()
    {
        gameObject.SetActive(false);
        Player.Instance.Respawn();
    }
}
