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
        if(hotbarSlotIndex != 0)
        {
            Player.Instance.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
        } else if (hotbarSlotIndex == 0)
        {
            Player.Instance.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }

        if (hotbarSlotIndex != 3)
        {
            Player.Instance.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (hotbarSlotIndex == 3)
        {
            Player.Instance.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        
        if (hotbarSlotIndex != 2)
        {
            Player.Instance.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (hotbarSlotIndex == 2)
        {
            Player.Instance.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
            
        if (hotbarSlotIndex != 1)
        {
            Player.Instance.transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (hotbarSlotIndex == 1)
        {
            Player.Instance.transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }

        _slots[_activeSlotIndex].SetActive(false);
        _activeSlotIndex = hotbarSlotIndex;
        _slots[_activeSlotIndex].SetActive(true);

    }
}
