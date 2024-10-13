using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ExtractorBuilding : FactoryBuilding
{
    [Header("Extractor settings")]
    [SerializeField] private ResourceTile[] _resourceTiles = Array.Empty<ResourceTile>();

    private ResourceTile _currentResourceTile;

    public Item Resource
    {
        get => _recipe.Result.Item;
        set => UpdateResource(value);
    }

    public TileBase Tile
    {
        get => _currentResourceTile.tile;
        set => UpdateTile(value);
    }

    private void UpdateResource(Item item)
    {
        if(_recipe == null)
            _recipe = ScriptableObject.CreateInstance<ItemRecipe>();

        _recipe.Result = new ItemStack(item, 1);
    }

    private void UpdateTile(TileBase tile)
    {
        _currentResourceTile = _resourceTiles.First(rt => rt.tile == tile);
        UpdateResource(_currentResourceTile.item);
    }

    protected override void Start()
    {
        base.Start();
    }

    private void OnValidate()
    {
        if(_recipe != null)
            throw new Exception("Extractor buildings cannot have recipes");
    }

    [Serializable]
    private struct ResourceTile
    {
        public TileBase tile;
        public Item item;
    }
}
