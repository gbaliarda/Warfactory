using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public ItemStack Stack { get; set; }
    private GraphicRaycaster _raycaster;
    private EventSystem _eventSystem;

    void Start()
    {
        _raycaster = GetComponentInParent<Canvas>().GetComponent<GraphicRaycaster>();
        _eventSystem = EventSystem.current;
    }

    void Update()
    {
        if (Stack != null && Input.GetMouseButtonDown(1))
        {
            PointerEventData pointerEventData = new(_eventSystem)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new();
            _raycaster.Raycast(pointerEventData, results);


            RaycastResult result = results.FirstOrDefault(r => r.gameObject == gameObject);

            if (result.gameObject != null && ChestUI.Instance.OpenChest)
            {
                EventManager.Instance.EventSaveItemInChest(this);
            }
        }
    }
}
