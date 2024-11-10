using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBuilding : Inventory, IDestroyable
{
    [SerializeField] private LayerMask _chestLayer;
    [SerializeField] private float _spawnInterval;
    [SerializeField] protected Transform _objectsContainer;
    [SerializeField] protected GameObject _itemEntityPrefab;
    private List<Vector3> _spawnItemPositions;
    private int _currentSpawnPosition;
    private bool _isSpawningItems;
    private bool _isOpen;

    private void OnEnable()
    {
        _onModified.AddListener(OnModified);
    }

    private void OnDisable()
    {
        _onModified.RemoveListener(OnModified);
    }

    private void Start()
    {
        if (_objectsContainer == null) _objectsContainer = GetComponentInParent<IZone>().ObjectsContainer;

        _spawnItemPositions = new();
        _currentSpawnPosition = 0;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && !Player.Instance.DeleteBuildingMode)
        {
            var cam = Camera.main;
            if (!cam) return;

            Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, _chestLayer);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                OpenChest();
            }
        }

        CheckNeighbourSliders();

        if (!_isSpawningItems && _spawnItemPositions.Count > 0)
        {
            _isSpawningItems = true;
            StartCoroutine(SpawnItemCoroutine());
        }

        if (_spawnItemPositions.Count == 0)
        {
            _isSpawningItems = false;
        }
    }

    private void CheckNeighbourSliders()
    {
        CheckNeighbourSlider(Vector3.up);
        CheckNeighbourSlider(Vector3.left);
        CheckNeighbourSlider(Vector3.right);
        CheckNeighbourSlider(Vector3.down);
    }

    private void CheckNeighbourSlider(Vector3 direction)
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position + direction, Vector2.zero);
        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent<Slider>(out var slider))
            {
                if (Vector3.Dot(direction.normalized, slider.Direction.normalized) > 0.9f && !_spawnItemPositions.Contains(direction))
                {
                    _spawnItemPositions.Add(direction);
                }
            }
        }
    }

    private IEnumerator SpawnItemCoroutine()
    {
        while (_isSpawningItems)
        {
            yield return new WaitForSeconds(_spawnInterval);

            if (!IsObjectPresent())
            {
                var offset = transform.rotation * _spawnItemPositions[_currentSpawnPosition];
                _currentSpawnPosition += 1;
                if (_currentSpawnPosition >= _spawnItemPositions.Count)
                    _currentSpawnPosition = 0;
                var item = PopLastItem();
                if (item == null)
                    continue;
                var go = Instantiate(_itemEntityPrefab, transform.position + offset, Quaternion.identity, _objectsContainer);
                var itemEntity = go.GetComponent<ItemEntity>();
                itemEntity.Stack = new ItemStack(item, 1);
            }

        }
    }

    private bool IsObjectPresent()
    {
        foreach (Transform child in _objectsContainer)
        {
            if (Vector3.Distance(transform.position + _spawnItemPositions[_currentSpawnPosition], child.position) < 1f)
            {
                return true;
            }
        }

        return false;
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

    public void Destroy()
    {
        Destroy(gameObject);
    }
}