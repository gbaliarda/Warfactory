using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryBuilding : MonoBehaviour
{
    #region Serialized Fields

    [Header("Stats")]
    [SerializeField] private Item _item;
    [SerializeField] private float _spawnInterval = 5f;
    [SerializeField] private float _overclock = 1f;

    [Header("Etc.")]
    [SerializeField] private Transform _objectsContainer;
    [SerializeField] private bool _isOn = true;
    [SerializeField] private GameObject _itemEntityPrefab;

    #endregion

    #region Properties

    public float RealTimeInterval { get; private set; } = 5f;
    public float Performance { get; private set; } = 100f;
    public float SpawnInterval => _spawnInterval;
    public float Overclock => _overclock;
    public bool IsOn => _isOn;

    #endregion

    private readonly Queue<float> _spawnTimes = new();
    private int _maxSpawnRecords = 5;

    public void SetOverclock(float value)
    {
        _overclock = value;
        if (_overclock == 0) RealTimeInterval = 0;
        else RealTimeInterval = _spawnInterval / _overclock;
    }
    
    public void SetIsOn(bool isOn)
    {
        _isOn = isOn;
        if (_isOn)
        {
            StartCoroutine(SpawnItemCoroutine());
        }
    }

    protected virtual void Start()
    {
        if (_objectsContainer == null) _objectsContainer = GameObject.Find("Objects").transform;

        StartCoroutine(SpawnItemCoroutine());
    }

    protected virtual IEnumerator SpawnItemCoroutine()
    {
        while (_isOn)
        {
            yield return new WaitForSeconds(_spawnInterval / _overclock);

            if (!IsObjectPresent())
            {
                var offset = transform.rotation * Vector3.up;
                var go = Instantiate(_itemEntityPrefab, transform.position + offset, Quaternion.identity, _objectsContainer);
                var itemEntity = go.GetComponent<ItemEntity>();
                itemEntity.Stack = new ItemStack(_item, 1);

                var currentTime = Time.time;
                if (_spawnTimes.Count >= _maxSpawnRecords)
                {
                    _spawnTimes.Dequeue();
                }
                _spawnTimes.Enqueue(currentTime);
            }

            CalculatePerformance();
        }
    }

    protected virtual bool IsObjectPresent()
    {
        foreach (Transform child in _objectsContainer)
        {
            if (Vector3.Distance(transform.position + Vector3.up, child.position) < 1f)
            {
                return true;
            }
        }

        return false;
    }

    protected virtual void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var cam = Camera.main;
            if(!cam) return;

            Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

            LayerMask buildingLayer = LayerMask.GetMask("Building");
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, buildingLayer);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                EventManager.Instance.EventOpenBuildingUI(this);
            }
        }
    }

    public float CalculatePerformance()
    {
        if (_spawnTimes.Count < 5) return 100f;

        var spawnTimesArray = _spawnTimes.ToArray();
        var firstTime = spawnTimesArray[0];
        var lastTime = spawnTimesArray[^1];
        var totalTime = lastTime - firstTime;

        var expectedSpawns = totalTime / (_spawnInterval / _overclock) + 1;

        float actualSpawns = _spawnTimes.Count;

        Performance = (actualSpawns / expectedSpawns) * 100f;
        return Mathf.Clamp(Performance, 0f, 100f);
    }
}
