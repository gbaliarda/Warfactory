using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour, IInventory
{
    [Header("Settings")]
    [SerializeField] protected int _stackCapacity = 20;
    [SerializeField] protected Item[] _allowedItems = Array.Empty<Item>();

    [Header("State")]
    [SerializeField] protected List<ItemStack> _stacks = new();

    [Header("Events")]
    [SerializeField] protected UnityEvent<IInventory> _onModified;

    public int StackCapacity => _stackCapacity;
    public Item[] AllowedItems => _allowedItems;
    public List<ItemStack> Stacks => _stacks;

    public ItemStack AddItemStack(ItemStack newStack)
    {
        if(_allowedItems.Length > 0 && !_allowedItems.Contains(newStack.Item))
        {
            Debug.Log("No se puede agregar el ítem al inventario.");
            return newStack;
        }

        foreach (var stack in _stacks)
        {
            if (stack.Item != newStack.Item ||
                stack.Amount >= stack.Item.MaxStackSize) continue;

            var remainingSpace = stack.Item.MaxStackSize - stack.Amount;
            var amountToAdd = Mathf.Min(remainingSpace, newStack.Amount);

            stack.IncreaseAmount(amountToAdd);
            newStack.DecreaseAmount(amountToAdd);

            if (newStack.Amount > 0) continue;

            _onModified?.Invoke(this);
            return newStack;
        }

        if (newStack.Amount <= 0 || _stacks.Count >= _stackCapacity)
        {
            Debug.Log("No se pudo agregar el ítem al inventario.");
            _onModified?.Invoke(this);
            return newStack;
        }

        _stacks.Add(newStack.Clone());
        newStack.DecreaseAmount(newStack.Amount);
        _onModified?.Invoke(this);
        return newStack;
    }

    public ItemStack AddItem(Item item, int amount = 1)
    {
        var newStack = new ItemStack(item, amount);
        return AddItemStack(newStack);
    }

    public bool RemoveItem(Item item, int amount = 1)
    {
        var stackIndex = _stacks.FindIndex(stack => stack.Item == item);
        if (stackIndex == -1) return false;

        var stack = _stacks[stackIndex];
        stack.DecreaseAmount(amount);
        _onModified?.Invoke(this);

        if (stack.Amount > 0) return true;

        _stacks.RemoveAt(stackIndex);
        _onModified?.Invoke(this);
        return true;
    }

    public bool RemoveItemStack(ItemStack stack)
    {
        var wasRemoved = _stacks.Remove(stack);
        _onModified?.Invoke(this);

        return wasRemoved;
    }

    public int GetItemAmount(Item item)
        => _stacks.Where(s => s.Item == item).Sum(s => s.Amount);
}
