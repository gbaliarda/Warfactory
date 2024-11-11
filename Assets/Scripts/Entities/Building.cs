using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IBuilding
{
    [SerializeField] private GameObject _buildingPrefab;
    [SerializeField] private int _scrapCost;
    [SerializeField] private Item _scrapItem;

    public GameObject BuildingPrefab => _buildingPrefab;
    public int ScrapCost => _scrapCost;

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

    public GameObject Build(Vector3 position, int rotation)
    {
        if (_buildingPrefab == null)
            return null;

        var inv = InventoryManager.Instance;
        var scrapAmount = inv.GetAmountOfItem(_scrapItem);
        if (scrapAmount < _scrapCost)
        {
            Debug.Log("Not enough scrap to build");
            return null;
        }

        inv.ConsumeItem(_scrapItem, _scrapCost);

        Quaternion targetRotation = Quaternion.Euler(0, 0, -90 * (rotation - 1));
        return Instantiate(_buildingPrefab, position, targetRotation, Player.Instance.CurrentZone.transform);
    }
}
