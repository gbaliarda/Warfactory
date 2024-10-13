using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Recipe", menuName = "Custom/Item Recipe")]
public class ItemRecipe : ScriptableObject
{
    [SerializeField] private ItemStack _result;
    [SerializeField] private ItemStack[] _ingredients;

    public ItemStack Result => _result;
    public ItemStack[] Ingredients => _ingredients;

    public bool CanCraft(IInventory inventory)
        => _ingredients.All(ingredient => inventory.GetItemAmount(ingredient.Item) >= ingredient.Amount);

    public ItemStack Craft(IInventory inventory)
    {
        if (!CanCraft(inventory)) return null;

        foreach (var ingredient in _ingredients)
            inventory.RemoveItem(ingredient.Item, ingredient.Amount);

        return _result.Clone();
    }
}
