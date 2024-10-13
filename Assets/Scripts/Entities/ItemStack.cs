using System;
using UnityEngine;

[Serializable]
public class ItemStack
{
    [SerializeField] private Item _item;
    [SerializeField] private int _amount;

    public Item Item => _item;
    public int Amount => _amount;

    public ItemStack(Item item, int amount)
    {
        _item = item;
        _amount = amount;
    }

    public void IncreaseAmount(int amount)
    {
        if (Amount + amount <= Item.MaxStackSize)
        { 
            _amount += amount;
        }
    }

    public void DecreaseAmount(int amount) {
        _amount -= amount;
    }


    public ItemStack Clone()
    {
        return new ItemStack(Item, Amount);
    }
}
