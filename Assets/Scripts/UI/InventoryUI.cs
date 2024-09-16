using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySlot[] _slots;
    private void Start()
    {
        _slots = transform.GetChild(0).GetComponentsInChildren<InventorySlot>();

        EventManager.Instance.OnOpenInventoryUI += OnOpenInventoryUI;
        EventManager.Instance.OnCloseInventoryUI += OnCloseInventoryUI;

        OnCloseInventoryUI();
    }

    private void OnOpenInventoryUI()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            GameObject slot = _slots[i].gameObject;
            List<Item> items = InventoryManager.Instance.Items;
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
            }
            else
            {
                Image itemImage = slot.transform.GetChild(0).GetComponent<Image>();
                Color color = itemImage.color;
                color.a = 0f;
                itemImage.color = color;
                TextMeshProUGUI itemStack = slot.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                itemStack.text = "";
            }
        }
        InventoryManager.Instance.SetIsOpen(true);
        gameObject.SetActive(true);
    }

    public void OnCloseInventoryUI()
    {
        InventoryManager.Instance.SetIsOpen(false);
        gameObject.SetActive(false);
    }
}
