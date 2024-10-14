using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private bool _isActive = true;
    [SerializeField] private Enemy[] _enemiesToSpawn;
    [SerializeField] private float _spawnCooldown= 20f;
    [SerializeField] private int _packAmount = 2;
    [SerializeField] private LayerMask _avoidLayers;

    void Start()
    {
        if (_isActive)
        {
            StartCoroutine(SpawnEnemiesRoutine());
        }
    }   

    private IEnumerator SpawnEnemiesRoutine()
    {
        yield return new WaitForSeconds(_spawnCooldown);

        while (_isActive)
        {
            yield return StartCoroutine(SpawnPackOfEnemies());
            yield return new WaitForSeconds(_spawnCooldown);
        }
    }

    private IEnumerator SpawnPackOfEnemies()
    {
        for (int i = 0; i < _packAmount; i++)
        {
            if (!IsAreaOccupied())
            {
                GameObject randomEnemy = _enemiesToSpawn[Random.Range(0, _enemiesToSpawn.Length)].gameObject;

                Instantiate(randomEnemy, transform.position, Quaternion.identity);

                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    private bool IsAreaOccupied()
    {
        return Physics2D.OverlapCircle(transform.position, 2f, _avoidLayers) != null;
    }

    public void SetSpawnerActive(bool isActive)
    {
        _isActive = isActive;

        if (_isActive)
        {
            StartCoroutine(SpawnEnemiesRoutine());
        }
        else
        {
            StopAllCoroutines();
        }
    }
}
