using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ChestUI : MonoBehaviour
{
    [SerializeField] private InventorySlot[] _slots;
    private void Start()
    {
        _slots = transform.GetChild(0).GetComponentsInChildren<InventorySlot>();

        EventManager.Instance.OnOpenChestUI += OnOpenChestUI;

        OnCloseChestUI();
    }

    private void OnOpenChestUI(List<Item> items)
    {
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
            } else
            {
                Image itemImage = slot.transform.GetChild(0).GetComponent<Image>();
                Color color = itemImage.color;
                color.a = 0f;
                itemImage.color = color;
                TextMeshProUGUI itemStack = slot.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                itemStack.text = "";
            }
        }
        gameObject.SetActive(true);
    }

    public void OnCloseChestUI()
    {
        gameObject.SetActive(false);
    }
}
