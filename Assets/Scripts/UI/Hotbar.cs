using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Hotbar : MonoBehaviour
{
    private int _activeSlotIndex = 0;
    private HotbarSlot[] _slots;

    private void Start()
    {
        _slots = GetComponentsInChildren<HotbarSlot>();

        EventManager.Instance.OnHotbarSlotChange += OnHotbarSlotChange;
        EventManager.Instance.OnInventoryUpdate += OnInventoryUpdate;

        OnHotbarSlotChange(0);
    }

    private void OnInventoryUpdate(Item item)
    {
        if (item is ShotgunBullet)
        {
            _slots[0].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = InventoryManager.Instance.GetAmountOfItemType<ShotgunBullet>().ToString();
        } else if (item is PotionItem)
        {
            _slots[4].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = InventoryManager.Instance.GetAmountOfItemType<PotionItem>().ToString();
        }
    }

    private void OnHotbarSlotChange(int hotbarSlotIndex)
    {
        Player.Instance.HotbarItems.GetChild(hotbarSlotIndex).gameObject.SetActive(true);

        if (hotbarSlotIndex != 0)
        {
            Player.Instance.HotbarItems.GetChild(0).gameObject.SetActive(false);
        }

        if (hotbarSlotIndex != 1)
        {
            Player.Instance.HotbarItems.GetChild(1).gameObject.SetActive(false);
        }
        
        if (hotbarSlotIndex != 2)
        {
            Player.Instance.HotbarItems.GetChild(2).gameObject.SetActive(false);
        }

        if (hotbarSlotIndex != 3)
        {
            Player.Instance.HotbarItems.GetChild(3).gameObject.SetActive(false);
        }

        if (hotbarSlotIndex != 4)
        {
            Player.Instance.HotbarItems.GetChild(4).gameObject.SetActive(false);
        }

        _slots[_activeSlotIndex].SetActive(false);
        _activeSlotIndex = hotbarSlotIndex;
        _slots[_activeSlotIndex].SetActive(true);

    }
}
