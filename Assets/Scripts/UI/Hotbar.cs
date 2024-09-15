using System.Collections;
using System.Collections.Generic;
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

        OnHotbarSlotChange(0);
    }

    private void OnHotbarSlotChange(int hotbarSlotIndex)
    {
        Player.Instance.transform.GetChild(hotbarSlotIndex).gameObject.SetActive(true);

        if (hotbarSlotIndex != 0)
        {
            Player.Instance.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (hotbarSlotIndex != 1)
        {
            Player.Instance.transform.GetChild(1).gameObject.SetActive(false);
        }
        
        if (hotbarSlotIndex != 2)
        {
            Player.Instance.transform.GetChild(2).gameObject.SetActive(false);
        }

        if (hotbarSlotIndex != 3)
        {
            Player.Instance.transform.GetChild(3).gameObject.SetActive(false);
        }

        if (hotbarSlotIndex != 4)
        {
            Player.Instance.transform.GetChild(4).gameObject.SetActive(false);
        }

        _slots[_activeSlotIndex].SetActive(false);
        _activeSlotIndex = hotbarSlotIndex;
        _slots[_activeSlotIndex].SetActive(true);

    }
}
