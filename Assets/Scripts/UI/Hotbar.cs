using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    private int _activeSlotIndex = 0;
    private HotbarSlot[] _slots;

    private void Start()
    {
        _slots = GetComponentsInChildren<HotbarSlot>();

        EventManager.Instance.OnHotbarSlotChange += OnHotbarSlotChange;
        EventManager.Instance.OnInventoryUpdate += OnInventoryUpdate;
        EventManager.Instance.OnBuildModeActive += OnBuildModeActive;

        OnHotbarSlotChange(0);
    }

    private void OnInventoryUpdate(ItemStack stack)
    {
        // if (item is ShotgunBullet)
        // {
        //     //_slots[0].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = InventoryManager.Instance.GetAmountOfItemType<ShotgunBullet>().ToString();
        // } else if (item is PotionItem)
        // {
        //     //_slots[1].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = InventoryManager.Instance.GetAmountOfItemType<PotionItem>().ToString();
        // }
    }

    private void OnHotbarSlotChange(int hotbarSlotIndex)
    {
        _slots[_activeSlotIndex].SetActive(false);
        _activeSlotIndex = hotbarSlotIndex;
        _slots[_activeSlotIndex].SetActive(true);

    }

    private void OnBuildModeActive(bool active)
    {
        if (active)
        {
            int i = 0;
            foreach (var building in Player.Instance.BuildHotbarItems.GetComponentsInChildren<Building>(true))
            {
                var spriteRenderer = building.GetComponent<SpriteRenderer>();

                Debug.Log(spriteRenderer.sprite);
                _slots[i].transform.GetChild(1).gameObject.SetActive(true);
                _slots[i].transform.GetChild(1).GetComponent<Image>().sprite = spriteRenderer.sprite;
                _slots[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";


                var scrapCostUI = _slots[i].transform.GetChild(3);
                scrapCostUI.gameObject.SetActive(true);
                scrapCostUI.GetChild(1).GetComponent<TextMeshProUGUI>().text = building.ScrapCost.ToString();

                i++;
            }
            for (; i < _slots.Length; i++)
            {
                _slots[i].transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        else
        {
            int i = 0;
            foreach (var spriteRenderer in Player.Instance.HotbarItems.GetComponentsInChildren<SpriteRenderer>(true))
            {
                _slots[i].transform.GetChild(1).gameObject.SetActive(true);
                _slots[i].transform.GetChild(1).GetComponent<Image>().sprite = spriteRenderer.sprite;
                _slots[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
                _slots[i].transform.GetChild(3).gameObject.SetActive(false);
                i++;
            }
            for (; i < _slots.Length; i++)
            {
                _slots[i].transform.GetChild(1).gameObject.SetActive(false);
                _slots[i].transform.GetChild(3).gameObject.SetActive(false);
            }
        }

        OnHotbarSlotChange(0);
    }
}
