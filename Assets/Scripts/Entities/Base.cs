using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Base : Singleton<Base>, IZone
{
    [SerializeField] private Transform _spawner;
    [SerializeField] private Collider2D _cameraConfiner;
    [SerializeField] private Tilemap _interactableMap;
    [SerializeField] private Tilemap _uiHoverMap;
    [SerializeField] private Tilemap _resourceMap;

    public Transform Spawner => _spawner;
    public Collider2D CameraConfiner => _cameraConfiner;
    [SerializeField] private Transform _objectsContainer;
    public Transform ObjectsContainer => _objectsContainer;
    void Start()
    {
        TileManager.Instance.SetInteractableMap(_interactableMap);
        TileManager.Instance.SetUIHoverMap(_uiHoverMap);
        TileManager.Instance.SetResourceMap(_resourceMap);
    }
}
