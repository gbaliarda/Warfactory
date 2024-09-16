using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public List<Item> Items => _items;
    public bool IsOpen => _isOpen;
    private bool _isOpen;
    private List<Item> _items;
    [SerializeField] private int _maxCapacity = 28;
    void Start()
    {
        EventManager.Instance.OnPickUpWorldObject += OnPickUpWorldObject;
        EventManager.Instance.OnPickUpChestItem += OnPickUpChestItem;

        _items = new();
        _isOpen = false;
    }

    public Item AddItem(Item item)
    {
        Item itemBackup = item.Clone();
        foreach (Item storedItem in _items)
        {
            if (storedItem.ItemId == item.ItemId && storedItem.StackAmount < storedItem.StackSize)
            {
                int remainingSpace = storedItem.StackSize - storedItem.StackAmount;
                int amountToAdd = Mathf.Min(remainingSpace, item.StackAmount);

                storedItem.IncreaseStack(amountToAdd);
                item.DecreaseStack(amountToAdd);

                Debug.Log($"{amountToAdd} {item.ItemName}(s) añadidos al stack existente, tamaño stack es {storedItem.StackAmount} hasta {storedItem.StackSize}.");

                if (item.StackAmount <= 0)
                {
                    if (IsOpen)
                    {
                        EventManager.Instance.EventOpenInventoryUI();
                    }
                    EventManager.Instance.EventInventoryUpdate(itemBackup);
                    return item;
                }
            }
        }

        if (item.StackAmount > 0 && _items.Count < _maxCapacity)
        {
            _items.Add(item.Clone());
            Debug.Log($"{item.StackAmount} {item.ItemName}(s) añadidos como nuevo stack.");
            item.DecreaseStack(item.StackAmount);
            if (IsOpen)
            {
                EventManager.Instance.EventOpenInventoryUI();
            }
            EventManager.Instance.EventInventoryUpdate(itemBackup);
            return item;
        }

        Debug.Log("No se pudo agregar el ítem al cofre.");
        if (IsOpen)
        {
            EventManager.Instance.EventOpenInventoryUI();
        }
        EventManager.Instance.EventInventoryUpdate(itemBackup);
        return item;
    }

    public bool RemoveItem(Item item)
    {
        if (_items.Contains(item))
        {
            _items.Remove(item);
            if (IsOpen)
            {
                EventManager.Instance.EventOpenInventoryUI();
            }
            EventManager.Instance.EventInventoryUpdate(item);
            return true;
        }

        if (IsOpen)
        {
            EventManager.Instance.EventOpenInventoryUI();
        }
        EventManager.Instance.EventInventoryUpdate(item);
        return false;
    }

    private void OnPickUpWorldObject(WorldObject worldObject)
    {
        Item leftItem = AddItem(worldObject.Item);
        if (leftItem.StackAmount <= 0)
        {
            Destroy(worldObject.gameObject);
        }
    }

    private void OnPickUpChestItem(ChestSlot chestSlot)
    {
        Item leftItem = AddItem(chestSlot.Item);
        
        ChestUI.Instance.OpenChest.RemoveItem(leftItem);
    }

    public void SetIsOpen(bool isOpen)
    {
        _isOpen = isOpen;
    }

    public int GetAmountOfItemType<T>() where T : Item
    {
        int totalAmount = 0;

        foreach (var item in _items)
        {
            if (item is T)
            {
                totalAmount += item.StackAmount;
            }
        }

        return totalAmount;
    }

    public int ConsumeItem<T>(int number) where T:Item
    {
        int remainingToConsume = number;

        for (int i = _items.Count - 1; i >= 0; i--)
        {
            if (_items[i] is T item)
            {
                if (item.StackAmount > remainingToConsume)
                {
                    item.DecreaseStack(remainingToConsume);
                    if (IsOpen)
                    {
                        EventManager.Instance.EventOpenInventoryUI();
                    }
                    EventManager.Instance.EventInventoryUpdate(item);
                    return 0;
                }
                else
                {
                    remainingToConsume -= item.StackAmount;
                    item.DecreaseStack(item.StackAmount);

                    if (item.StackAmount <= 0)
                    {
                        _items.RemoveAt(i);
                        if (IsOpen)
                        {
                            EventManager.Instance.EventOpenInventoryUI();
                        }
                    }

                    if (remainingToConsume == 0)
                    {
                        EventManager.Instance.EventInventoryUpdate(item);
                        return 0;
                    }
                }
            }
        }

        return remainingToConsume;
    }
}
