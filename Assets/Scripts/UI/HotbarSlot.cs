using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour
{
    public Transform Active => _active;
    private Transform _active;

    public Transform ItemImage => _itemImage;
    private Transform _itemImage;

    private Button _button;

    void Start()
    {
        _active= transform.Find("Active");
        _itemImage = transform.Find("Item");

        if (TryGetComponent(out _button))
        {
            _button.onClick.AddListener(OnClick);
        }
    }

    public void SetActive(bool isActive)
    {
        if (_active != null)
        {
            _active.gameObject.SetActive(isActive);
            if (isActive)
            {
                EventManager.Instance.EventHotbarItemSelect(gameObject);
            }
        }
    }

    public void SetItemSprite(Sprite itemSprite)
    {
        if (_itemImage != null)
        {
            if (_itemImage.TryGetComponent<Image>(out var itemImage))
            {
                itemImage.sprite = itemSprite;
                _itemImage.gameObject.SetActive(itemSprite != null);
            }
        }
    }

    private void OnClick()
    {
        int siblingIndex = transform.GetSiblingIndex();
        EventManager.Instance.EventHotbarSlotChange(siblingIndex);
    }
}
