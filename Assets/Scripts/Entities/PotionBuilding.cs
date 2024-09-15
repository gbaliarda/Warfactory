using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionBuilding : MonoBehaviour
{
    [SerializeField] private GameObject _potionToFabricate;
    [SerializeField] private float _spawnInterval = 5f;
    [SerializeField] private bool _isOn = true;
    [SerializeField] private Transform _objectsContainer;
    [SerializeField] private Sprite _potionSprite;

    void Start()
    {
        if (_objectsContainer == null) _objectsContainer = GameObject.Find("Objects").transform;

        StartCoroutine(SpawnPotionCoroutine());
    }

    private IEnumerator SpawnPotionCoroutine()
    {
        while (_isOn)
        {
            yield return new WaitForSeconds(_spawnInterval);

            if (!IsObjectPresent())
            {
                GameObject potion = Instantiate(_potionToFabricate, transform.position + Vector3.up, Quaternion.identity, _objectsContainer);
                potion.GetComponent<WorldObject>().Item = new PotionItem(2, "Potion", _potionSprite, 5, ItemRarity.Common);
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
