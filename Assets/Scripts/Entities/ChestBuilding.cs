using UnityEngine;

public class ChestBuilding : Inventory
{
    [SerializeField] private LayerMask _chestLayer;
    private bool _isOpen;

    private void OnEnable()
    {
        _onModified.AddListener(OnModified);
    }

    private void OnDisable()
    {
        _onModified.RemoveListener(OnModified);
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(1)) return;

        var cam = Camera.main;
        if (!cam) return;

        Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, _chestLayer);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            OpenChest();
        }
    }

    private void OnModified(IInventory inv)
    {
        if (!_isOpen) return;
        ChestUI.Instance.UpdateItemsInChestUI();
    }

    public void OpenChest()
    {
        Debug.Log("Open Chest");
        _isOpen = true;
        EventManager.Instance.EventOpenChestUI(this);
    }

    public void CloseChest()
    {
        _isOpen = false;
    }
}