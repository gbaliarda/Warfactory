using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChestBuilding : MonoBehaviour
{
    [SerializeField] private List<ItemStack> _storedStacks = new();
    [SerializeField] private int _maxCapacity = 20;
    [SerializeField] private LayerMask _chestLayer;
    private bool _isOpen;

    public List<ItemStack> StoredStacks => _storedStacks;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, _chestLayer);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Debug.Log("Open Chest");
                _isOpen = true;
                EventManager.Instance.EventOpenChestUI(this);
            }
        }
    }

    public void CloseChest()
    {
        _isOpen = false;
    }

    public ItemStack AddItemStack(ItemStack newItemStack)
    {
        foreach (ItemStack storedItem in _storedStacks)
        {
            if (storedItem.Item != newItemStack.Item ||
                storedItem.Amount >= storedItem.Item.MaxStackSize) continue;

            var remainingSpace = storedItem.Item.MaxStackSize - storedItem.Amount;
            var amountToAdd = Mathf.Min(remainingSpace, newItemStack.Amount);

            storedItem.IncreaseAmount(amountToAdd);
            newItemStack.DecreaseAmount(amountToAdd);

            if (newItemStack.Amount > 0) continue;

            if (_isOpen) ChestUI.Instance.UpdateItemsInChestUI();
            return newItemStack;
        }

        if (newItemStack.Amount > 0 && _storedStacks.Count < _maxCapacity)
        {
            _storedStacks.Add(newItemStack.Clone());
            newItemStack.DecreaseAmount(newItemStack.Amount);
            if (_isOpen) ChestUI.Instance.UpdateItemsInChestUI();
            return newItemStack;
        }

        Debug.Log("No se pudo agregar el �tem al cofre.");
        if (_isOpen) ChestUI.Instance.UpdateItemsInChestUI();
        return newItemStack;
    }

    public ItemStack AddItem(Item item, int amount = 1)
    {
        var newItemStack = new ItemStack(item, amount);
        return AddItemStack(newItemStack);
    }

    public bool RemoveItem(Item item)
    {
        var indexToRemove = _storedStacks.FindIndex(i => i.Item == item);
        if (indexToRemove == -1) return false;

        _storedStacks.RemoveAt(indexToRemove);
        if (_isOpen) ChestUI.Instance.UpdateItemsInChestUI();
        return true;
    }

    public bool RemoveItemStack(ItemStack stack)
    {
        var wasRemoved = _storedStacks.Remove(stack);
        if(_isOpen) ChestUI.Instance.UpdateItemsInChestUI();

        return wasRemoved;
    }
}
