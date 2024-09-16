using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ChestUI : Singleton<ChestUI>
{
    [SerializeField] private ChestSlot[] _slots;
    public ChestBuilding OpenChest { get; private set; }
    private void Start()
    {
        _slots = transform.GetChild(0).GetComponentsInChildren<ChestSlot>();

        EventManager.Instance.OnOpenChestUI += OnOpenChestUI;

        OnCloseChestUI();
    }

    private void OnOpenChestUI(ChestBuilding chest)
    {
        OpenChest = chest;
        UpdateItemsInChestUI();
        gameObject.SetActive(true);
    }

    public void UpdateItemsInChestUI()
    {
        List<Item> items = OpenChest.StoredItems;
        for (int i = 0; i < _slots.Length; i++)
        {
            GameObject slot = _slots[i].gameObject;
            if (i < items.Count)
            {
                Item item = items[i];
                Image itemImage = slot.transform.GetChild(0).GetComponent<Image>();
                itemImage.sprite = item.ItemImage;
                Color color = itemImage.color;
                color.a = 1f;
                itemImage.color = color;
                TextMeshProUGUI itemStack = slot.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                itemStack.text = item.StackAmount.ToString();
                _slots[i].Item = item;
            }
            else
            {
                Image itemImage = slot.transform.GetChild(0).GetComponent<Image>();
                Color color = itemImage.color;
                color.a = 0f;
                itemImage.color = color;
                TextMeshProUGUI itemStack = slot.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                itemStack.text = "";
                _slots[i].Item = null;
            }
        }
    }

    public void OnCloseChestUI()
    {
        if (OpenChest != null) OpenChest.CloseChest();
        gameObject.SetActive(false);
    }
}
