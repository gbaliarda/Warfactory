using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChestSlot : MonoBehaviour
{
    public Item Item { get; set; }
    private GraphicRaycaster _raycaster;
    private EventSystem _eventSystem;

    void Start()
    {
        _raycaster = GetComponentInParent<Canvas>().GetComponent<GraphicRaycaster>();
        _eventSystem = EventSystem.current;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            PointerEventData pointerEventData = new PointerEventData(_eventSystem)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new();

            _raycaster.Raycast(pointerEventData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject == gameObject)
                {
                    if (Item != null)
                    {
                        EventManager.Instance.EventPickUpChestItem(this);
                    }
                }
            }
        }
    }
}
