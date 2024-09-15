using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    public Sprite ItemImage { get; protected set; }
    public int ItemId { get; protected set; }
    public int StackSize { get; protected set; }
    public int StackAmount { get; protected set; }
    public string ItemName { get; protected set; }
    public string ItemDescription { get; protected set; }
    public ItemRarity Rarity { get; protected set; }
    public bool IsConsumable { get; protected set; }
    public bool IsEquipable { get; protected set; }

    public void IncreaseStack(int amount)
    {
        if (StackAmount + amount <= StackSize)
        { 
            StackAmount += amount;
        }
    }

    public void DecreaseStack(int amount) {
        StackAmount -= amount;
    }

    public abstract Item Clone();
}

public enum ItemRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

