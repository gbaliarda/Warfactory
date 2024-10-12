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
            List<ItemStack> items = InventoryManager.Instance.Stacks;
            if (i < items.Count)
            {
                var item = items[i];

                var image = slot.transform.GetChild(0).GetComponent<Image>();
                image.sprite = item.Item.Sprite;
                var color = image.color;
                color.a = 1f;
                image.color = color;

                var stackAmountText = slot.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                stackAmountText.text = item.Amount.ToString();
            }
            else
            {
                var image = slot.transform.GetChild(0).GetComponent<Image>();
                var color = image.color;
                color.a = 0f;
                image.color = color;

                var amountText = slot.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                amountText.text = "";
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
