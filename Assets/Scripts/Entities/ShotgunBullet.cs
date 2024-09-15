using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullet : Item
{
    public ShotgunBullet(int itemId, string itemName, Sprite itemImage, int stackSize, ItemRarity rarity)
    {
        ItemId = itemId;
        ItemName = itemName;
        ItemImage = itemImage;
        StackSize = stackSize;
        StackAmount = 1;
        Rarity = rarity;
        IsConsumable = false;
        IsEquipable = false;
    }

    public override Item Clone()
    {
        return new ShotgunBullet(ItemId, ItemName, ItemImage, StackSize, Rarity);
    }
}
