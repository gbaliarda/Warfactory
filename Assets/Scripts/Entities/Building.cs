using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IBuilding
{
    public GameObject BuildingPrefab => _buildingPrefab;
    [SerializeField] private GameObject _buildingPrefab;
    public void Destroy()
    {
        throw new System.NotImplementedException();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Build(Vector3 position)
    {
        if (_buildingPrefab == null)
            return;

        Instantiate(_buildingPrefab, position, Quaternion.identity);
    }
}
