using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Item", fileName = "New Item")]
public class Item : ScriptableObject
{
    #region Serialized Fields

    [Header("Visualization")]
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string _name;
    [SerializeField] private string _description;

    [Header("Properties")]
    [SerializeField] private int _maxStackSize;
    [SerializeField] private ItemRarity _rarity;
    [SerializeField] private bool _isConsumable;
    [SerializeField] private bool _isEquipable;

    #endregion

    #region Public Properties

    public Sprite Sprite => _sprite;
    public string Name => _name;
    public string Description => _description;
    public int MaxStackSize => _maxStackSize;
    public ItemRarity Rarity => _rarity;
    public bool IsConsumable => _isConsumable;
    public bool IsEquipable => _isEquipable;

    #endregion
}

public enum ItemRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}
