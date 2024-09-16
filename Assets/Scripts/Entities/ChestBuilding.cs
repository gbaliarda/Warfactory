using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBuilding : MonoBehaviour
{
    [SerializeField] private List<Item> _storedItems = new();
    [SerializeField] private int _maxCapacity = 20;
    [SerializeField] private LayerMask _chestLayer;
    private bool _isOpen;

    public List<Item> StoredItems => _storedItems;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, _chestLayer);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Debug.Log("Open Chest");
                EventManager.Instance.EventOpenChestUI(this);
            }
        }
    }

    public Item AddItem(Item item)
    {
        foreach (Item storedItem in _storedItems)
        {
            if (storedItem.ItemId == item.ItemId && storedItem.StackAmount < storedItem.StackSize)
            {
                int remainingSpace = storedItem.StackSize - storedItem.StackAmount;
                int amountToAdd = Mathf.Min(remainingSpace, item.StackAmount);

                storedItem.IncreaseStack(amountToAdd);
                item.DecreaseStack(amountToAdd);

                if (item.StackAmount <= 0)
                {
                    ChestUI.Instance.UpdateItemsInChestUI();
                    return item;
                }
            }
        }

        if (item.StackAmount > 0 && _storedItems.Count < _maxCapacity)
        {
            _storedItems.Add(item.Clone());
            item.DecreaseStack(item.StackAmount);
            ChestUI.Instance.UpdateItemsInChestUI();
            return item;
        }

        Debug.Log("No se pudo agregar el ítem al cofre.");
        ChestUI.Instance.UpdateItemsInChestUI();
        return item;
    }

    public bool RemoveItem(Item item)
    {
        if (_storedItems.Contains(item))
        {
            _storedItems.Remove(item);
            ChestUI.Instance.UpdateItemsInChestUI();
            return true;
        }

        return false;
    }

    public List<Item> GetStoredItems()
    {
        return _storedItems;
    }
}
