using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : Singleton<TileManager>
{
    [SerializeField] private Tilemap _interactableMap;

    [SerializeField] private Tile _hiddenInteractableTile;
    [SerializeField] private Tile _interactedTile;
    [SerializeField] private Tile _tileToReplace;
    void Start()
    {
        foreach(var position in _interactableMap.cellBounds.allPositionsWithin)
        {
            TileBase currentTile = _interactableMap.GetTile(position);

            if (currentTile == _tileToReplace)
            {
                _interactableMap.SetTile(position, _hiddenInteractableTile);
            }
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

    public void SetInteractable(Vector3Int position)
    {
        _interactableMap.SetTile(position, _interactedTile);
    }
}
