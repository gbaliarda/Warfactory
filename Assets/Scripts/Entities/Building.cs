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

    public void Build(Vector3 position, int rotation)
    {
        if (_buildingPrefab == null)
            return;

        Quaternion targetRotation = Quaternion.Euler(0, 0, -90 * (rotation - 1));

        Instantiate(_buildingPrefab, position, targetRotation, Player.Instance.CurrentZone.transform);
    }
}
