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
    }

    private void OnHotbarSlotChange(int hotbarSlotIndex)
    {
        if(hotbarSlotIndex != 0 && _activeSlotIndex == 0)
        {
            Player.Instance.transform.GetChild(0).gameObject.SetActive(false);
        } else if (hotbarSlotIndex == 0)
        {
            Player.Instance.transform.GetChild(0).gameObject.SetActive(true);
        }

        _slots[_activeSlotIndex].SetActive(false);
        _activeSlotIndex = hotbarSlotIndex;
        _slots[_activeSlotIndex].SetActive(true);

    }
}
