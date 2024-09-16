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
                    Debug.Log("Item agregado al cofre");
                    if (InventoryManager.Instance.IsOpen)
                    {
                        EventManager.Instance.EventOpenInventoryUI();
                    }
                    return item;
                }
            }
        }

        if (item.StackAmount > 0 && _items.Count < _maxCapacity)
        {
            _items.Add(item.Clone());
            Debug.Log($"{item.StackAmount} {item.ItemName}(s) añadidos como nuevo stack.");
            item.DecreaseStack(item.StackAmount);
            if (InventoryManager.Instance.IsOpen)
            {
                EventManager.Instance.EventOpenInventoryUI();
            }
            return item;
        }

        Debug.Log("No se pudo agregar el ítem al cofre.");
        if (InventoryManager.Instance.IsOpen)
        {
            EventManager.Instance.EventOpenInventoryUI();
        }
        return item;
    }

    public bool RemoveItem(Item item)
    {
        if (_items.Contains(item))
        {
            _items.Remove(item);
            Debug.Log($"{item.ItemName} fue removido del cofre.");
            return true;
        }

        Debug.Log($"{item.ItemName} no está en el cofre.");
        return false;
    }

    private void OnPickUpItem(Item item)
    {
        AddItem(item);
    }

    private void OnPickUpWorldObject(WorldObject worldObject)
    {
        Item leftItem = AddItem(worldObject.Item);
        Debug.Log($"Old item stack amount was {worldObject.Item.StackAmount}, new amount is {leftItem.StackAmount}");
        if (leftItem.StackAmount <= 0)
        {
            Debug.Log("Destroying Item");
            Destroy(worldObject.gameObject);
        }
    }

    private void OnPickUpChestItem(ChestSlot chestSlot)
    {
        Item leftItem = AddItem(chestSlot.Item);
        Debug.Log($"Old item stack amount was {chestSlot.Item.StackAmount}, new amount is {leftItem.StackAmount}");
        
        ChestUI.Instance.OpenChest.RemoveItem(leftItem);
    }

    public void SetIsOpen(bool isOpen)
    {
        _isOpen = isOpen;
    }
}
