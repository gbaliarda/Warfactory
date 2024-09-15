using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionItem : Item
{
    public PotionItem(int itemId, string itemName, Sprite itemImage, int stackSize, ItemRarity rarity)
    {
        ItemId = itemId;
        ItemName = itemName;
        ItemImage = itemImage;
        StackSize = stackSize;
        StackAmount = 1;
        Rarity = rarity;
        IsConsumable = true;
        IsEquipable = false;
    }

    public override Item Clone()
    {
        return new PotionItem(ItemId, ItemName, ItemImage, StackSize, Rarity);
    }
}
