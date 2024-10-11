using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TemporalLevel : Singleton<TemporalLevel>
{
    [SerializeField] private Transform _spawner;
    [SerializeField] private Collider2D _cameraConfiner;
    [SerializeField] private Tilemap _interactableMap;
    [SerializeField] private Tilemap _uiHoverMap;

    public Transform Spawner => _spawner;
    public Collider2D CameraConfiner => _cameraConfiner;
    void Start()
    {
        TileManager.Instance.SetInteractableMap(_interactableMap);
        TileManager.Instance.SetUIHoverMap(_uiHoverMap);
        TileManager.Instance.InstantiateInteractableMap();
    }

    void Update()
    {
        
    }
}
