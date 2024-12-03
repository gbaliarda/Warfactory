using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : Singleton<TileManager>
{
    [SerializeField] private Tilemap _interactableMap;
    [SerializeField] private Tilemap _uiHoverMap;
    [SerializeField] private Tilemap _resourceMap;
    [SerializeField] private Tilemap _grassMap;
    [SerializeField] private Tilemap _cosmeticMap;

    [SerializeField]  private Tilemap _baseInteractableMap;
    [SerializeField]  private Tilemap _baseUiHoverMap;
    [SerializeField]  private Tilemap _baseResourceMap;
    [SerializeField]  private Tilemap _baseGrassMap;
    [SerializeField]  private Tilemap _baseCosmeticMap;

    [SerializeField] private Tile _hiddenInteractableTile;
    [SerializeField] private Tile _uiHoverTile;
    [SerializeField] private Tile _interactedTile;
    [SerializeField] private Tile _occupiedTile;
    [SerializeField] private Tile _tileToReplace;

    private Vector3Int? _oldCursorPosition;
    private GameObject _extractorToolbar;

    void Start()
    {
        _interactableMap = _baseInteractableMap;
        _uiHoverMap = _baseUiHoverMap;
        _resourceMap = _baseResourceMap;
        _grassMap = _baseGrassMap;
        _cosmeticMap = _baseCosmeticMap;
        InstantiateInteractableMap();
    }

    public void InstantiateInteractableMap()
    {
        foreach (var position in _interactableMap.cellBounds.allPositionsWithin)
        {
            TileBase currentTile = _interactableMap.GetTile(position);

            if (currentTile == _tileToReplace)
            {
                _interactableMap.SetTile(position, _hiddenInteractableTile);
            }
        }

        _extractorToolbar = Player.Instance.BuildHotbarItems.Find("ExtractorBuilding").gameObject;
    }

    public void SetInteractableMap(Tilemap interactableMap)
    {
        Debug.Log("Updating interactable tilemap to "+ interactableMap.name);
        _interactableMap = interactableMap;
    }

    public void SetUIHoverMap(Tilemap hoverMap)
    {
        _uiHoverMap = hoverMap;
    }

    public void SetResourceMap(Tilemap resourceMap)
    {
        _resourceMap = resourceMap;
    }
    public void SetGrassMap (Tilemap grassMap)
    {
        _grassMap = grassMap;
    }
    
    public void SetCosmeticMap (Tilemap cosmeticMap)
    {
        _cosmeticMap = cosmeticMap;
    }

    private void Update()
    {
        if (Player.Instance.BuildingMode)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int layerToIgnore = LayerMask.GetMask("Camera");
            int layerMask = ~layerToIgnore;
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, layerMask);
            if (hit.collider == null)
            {
                hideCursor();
                return;
            }
            Vector3 hoverPosition = hit.point;
            GameObject hoverObject = hit.collider.gameObject;

            if (hoverObject.TryGetComponent<Tilemap>(out var tilemap))
            {
                Vector3Int cellPosition = tilemap.WorldToCell(hoverPosition);
                TileBase currentTile = _uiHoverMap.GetTile(cellPosition);

                var isResource = IsResource(cellPosition);
                var extractorInHand = _extractorToolbar.gameObject.activeSelf;

                if ( (!extractorInHand || isResource) &&
                     (currentTile == null || currentTile.name != "ArrowBox"))
                {
                    if (_oldCursorPosition != null) _uiHoverMap.SetTile(_oldCursorPosition.Value, null);
                    _oldCursorPosition = cellPosition;
                    UpdateTile(cellPosition);
                }

                if (extractorInHand && !isResource) hideCursor();
            } else
            {
                hideCursor();
            }
        } else
        {
            hideCursor();
        }
    }

    private void UpdateTile(Vector3Int cellPosition)
    {
        if (_oldCursorPosition != null) _uiHoverMap.SetTile(_oldCursorPosition.Value, null);
        int rotationIndex = Player.Instance.BuildingRotation;
        float angle = 0f;

        switch (rotationIndex)
        {
            case 2: angle = 270f; break;
            case 3: angle = 180f; break;
            case 4: angle = 90f; break;
        }
        Debug.Log("Setting Tile");
        _uiHoverMap.SetTile(cellPosition, _uiHoverTile);
        Matrix4x4 rotationMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 0, angle));
        _uiHoverMap.SetTransformMatrix(cellPosition, rotationMatrix);
    }

    public void OnBuildingRotationChanged()
    {
        if (_oldCursorPosition != null)
        {
            UpdateTile(_oldCursorPosition.Value);
        }
    }

    void hideCursor()
    {
        if (_oldCursorPosition != null)
        {
            _uiHoverMap.SetTile(_oldCursorPosition.Value, null);
            _oldCursorPosition = null;
        }
    }

    public bool IsInteractable(Vector3Int position)
    {
        TileBase tile = _interactableMap.GetTile(position);

        if(tile != null)
        {
            if (tile.name == "Interactable")
            {
                return true;
            }
        }

        return false;
    }

    public bool IsResource(Vector3Int position)
    {
        var tile = GetResourceTile(position);

        return tile != null;
    }

    public TileBase GetResourceTile(Vector3Int position)
    {
        return _resourceMap.GetTile(position);
    }

    public void SetInteractable(Vector3Int position)
    {
        _interactableMap.SetTile(position, _interactedTile);
    }

    public void SetOccupied(Vector3Int position)
    {
        _interactableMap.SetTile(position, _occupiedTile);
    }
    
    public void SetUnoccupied(Vector2 raycastHitPoint)
    {
        var cellPosition = _interactableMap.WorldToCell(raycastHitPoint);
        _interactableMap.SetTile(cellPosition, _hiddenInteractableTile);
    }

    public void RestoreBaseTilemaps()
    {
        _interactableMap = _baseInteractableMap;
        _uiHoverMap = _baseUiHoverMap;
        _resourceMap = _baseResourceMap;
        _grassMap = _baseGrassMap;
        _cosmeticMap = _baseCosmeticMap;
    }

    public int TileType(Vector3 position)
    {
        TileBase cosmeticTile = _cosmeticMap.GetTile(_cosmeticMap.WorldToCell(position));
        TileBase grassTile = _grassMap.GetTile(_grassMap.WorldToCell(position));

        if (cosmeticTile != null)
        {
            return 2;
        } else if (grassTile != null)
        {
            return 1;
        }

        return -1;
    }
}
