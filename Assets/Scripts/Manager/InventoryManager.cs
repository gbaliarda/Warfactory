using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public List<ItemStack> Stacks { get; private set; }
    public bool IsOpen { get; private set; }

    [SerializeField] private int _maxCapacity = 28;
    void Start()
    {
        EventManager.Instance.OnPickUpWorldObject += OnPickUpItemEntity;
        EventManager.Instance.OnPickUpChestItem += OnPickUpChestStack;

        Stacks = new();
        IsOpen = false;
    }

    public ItemStack AddItemStack(ItemStack newStack)
    {
        var itemBackup = newStack.Clone();
        foreach (var storedStack in Stacks)
        {
            if (storedStack.Item == newStack.Item && storedStack.Amount < storedStack.Item.MaxStackSize)
            {
                int remainingSpace = storedStack.Item.MaxStackSize - storedStack.Amount;
                int amountToAdd = Mathf.Min(remainingSpace, newStack.Amount);

                storedStack.IncreaseAmount(amountToAdd);
                newStack.DecreaseAmount(amountToAdd);

                Debug.Log($"{amountToAdd} {newStack.Item.Name}(s) añadidos al stack existente, tamaño stack es {storedStack.Amount} hasta {storedStack.Item.MaxStackSize}.");

                if (newStack.Amount <= 0)
                {
                    if (IsOpen)
                    {
                        EventManager.Instance.EventOpenInventoryUI();
                    }
                    EventManager.Instance.EventInventoryUpdate(itemBackup);
                    return newStack;
                }
            }
        }

        if (newStack.Amount > 0 && Stacks.Count < _maxCapacity)
        {
            Stacks.Add(newStack.Clone());
            Debug.Log($"{newStack.Amount} {newStack.Item.Name}(s) añadidos como nuevo stack.");
            newStack.DecreaseAmount(newStack.Amount);
            if (IsOpen)
            {
                EventManager.Instance.EventOpenInventoryUI();
            }
            EventManager.Instance.EventInventoryUpdate(itemBackup);
            return newStack;
        }

        Debug.Log("No se pudo agregar el ítem al cofre.");
        if (IsOpen)
        {
            EventManager.Instance.EventOpenInventoryUI();
        }
        EventManager.Instance.EventInventoryUpdate(itemBackup);
        return newStack;
    }

    public bool RemoveItemStack(ItemStack stack)
    {
        var wasRemoved = Stacks.Remove(stack);

        if (IsOpen)
        {
            EventManager.Instance.EventOpenInventoryUI();
        }
        EventManager.Instance.EventInventoryUpdate(stack);

        return wasRemoved;
    }

    private void OnPickUpItemEntity(ItemEntity itemEntity)
    {
        var leftItem = AddItemStack(itemEntity.Stack);
        if (leftItem.Amount <= 0)
        {
            Destroy(itemEntity.gameObject);
        }
    }

    private void OnPickUpChestStack(ChestSlot chestSlot)
    {
        var leftStack = AddItemStack(chestSlot.Stack);

        ChestUI.Instance.OpenChest.RemoveItemStack(leftStack);
    }

    public void SetIsOpen(bool isOpen)
    {
        IsOpen = isOpen;
    }

    public int GetAmountOfItem(Item item)
    {
        return Stacks.Where(stack => stack.Item == item).Sum(stack => stack.Amount);
    }

    public int ConsumeItem(Item item, int number)
    {
        var remainingToConsume = number;

        for (var i = Stacks.Count - 1; i >= 0; i--)
        {
            var stack = Stacks[i];
            if (stack.Item != item) continue;

            if (stack.Amount > remainingToConsume)
            {
                stack.DecreaseAmount(remainingToConsume);
                if (IsOpen)
                {
                    EventManager.Instance.EventOpenInventoryUI();
                }
                EventManager.Instance.EventInventoryUpdate(stack);
                return 0;
            }

            remainingToConsume -= stack.Amount;
            stack.DecreaseAmount(stack.Amount);

            if (stack.Amount <= 0)
            {
                Stacks.RemoveAt(i);
                if (IsOpen)
                {
                    EventManager.Instance.EventOpenInventoryUI();
                }
            }

            if (remainingToConsume != 0) continue;

            EventManager.Instance.EventInventoryUpdate(stack);
            return 0;
        }

        return remainingToConsume;
    }
}
