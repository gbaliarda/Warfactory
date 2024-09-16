using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBuilding : MonoBehaviour
{
    [SerializeField] private GameObject _bulletToFabricate;
    [SerializeField] private Transform _objectsContainer;
    [SerializeField] private Sprite _bulletSprite;
    [SerializeField] private float _spawnInterval = 5f;
    [SerializeField] private bool _isOn = true;

    void Start()
    {
        if (_objectsContainer == null) _objectsContainer = GameObject.Find("Objects").transform;

        StartCoroutine(SpawnBulletCoroutine());
    }

    private IEnumerator SpawnBulletCoroutine()
    {
        while (_isOn)
        {
            yield return new WaitForSeconds(_spawnInterval);

            if (!IsObjectPresent())
            {
                GameObject bullet = Instantiate(_bulletToFabricate, transform.position + Vector3.up, Quaternion.identity, _objectsContainer);
                bullet.GetComponent<WorldObject>().Item = new ShotgunBullet(1, "Shotgun Bullet", _bulletSprite, 10, ItemRarity.Common);
            }
        }
    }

    private bool IsObjectPresent()
    {

        foreach (Transform child in _objectsContainer)
        {
            if (Vector3.Distance(transform.position + Vector3.up, child.position) < 1f)
            {
                return true;
            }
        }

        return false;
    }
}
