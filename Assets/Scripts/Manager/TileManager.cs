using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : Singleton<TileManager>
{
    [SerializeField] private Tilemap _interactableMap;
    [SerializeField] private Tilemap _uiHoverMap;
    [SerializeField] private Tilemap _resourceMap;

    [SerializeField]  private Tilemap _baseInteractableMap;
    [SerializeField]  private Tilemap _baseUiHoverMap;
    [SerializeField]  private Tilemap _baseResourceMap;

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
        Debug.Log("Updating interactable tilemap");
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
                     (currentTile == null || currentTile.name != "BoxWhiteOutlineSquared"))
                {
                    if (_oldCursorPosition != null) _uiHoverMap.SetTile(_oldCursorPosition.Value, null);
                    _uiHoverMap.SetTile(cellPosition, _uiHoverTile);
                    _oldCursorPosition = cellPosition;
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

    public void RestoreBaseTilemaps()
    {
        _interactableMap = _baseInteractableMap;
        _uiHoverMap = _baseUiHoverMap;
        _resourceMap = _baseResourceMap;
    }
}
