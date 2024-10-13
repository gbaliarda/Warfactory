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
        var stacks = OpenChest.Stacks;
        for (int i = 0; i < _slots.Length; i++)
        {
            var slot = _slots[i].gameObject;
            if (i < stacks.Count)
            {
                var stack = stacks[i];

                var image = slot.transform.GetChild(0).GetComponent<Image>();
                image.sprite = stack.Item.Sprite;
                var color = image.color;
                color.a = 1f;
                image.color = color;

                var amountText = slot.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                amountText.text = stack.Amount.ToString();
                _slots[i].Stack = stack;
            }
            else
            {
                Image itemImage = slot.transform.GetChild(0).GetComponent<Image>();
                Color color = itemImage.color;
                color.a = 0f;
                itemImage.color = color;
                TextMeshProUGUI itemStack = slot.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                itemStack.text = "";
                _slots[i].Stack = null;
            }
        }
    }

    public void OnCloseChestUI()
    {
        if (OpenChest != null) OpenChest.CloseChest();
        gameObject.SetActive(false);
    }
}
