using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawners : MonoBehaviour
{
    [SerializeField] private EnemySpawner[] spawners;

    void Start()
    {
        spawners = GetComponentsInChildren<EnemySpawner>();
    }

    public void ActiveAllSpawners()
    {
        foreach (EnemySpawner spawner in spawners)
        {
            spawner.SetSpawnerActive(true);
        }
    }

    public void InactiveAllSpawners()
    {
        foreach (EnemySpawner spawner in spawners)
        {
            spawner.SetSpawnerActive(false);
        }
    }
}
