using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class FactoryBuilding : MonoBehaviour, IDamageable, IDestroyable
{
    #region Serialized Fields

    [Header("Stats")]
    [SerializeField] protected ItemRecipe _recipe;
    [SerializeField] protected BuildingStats stats;

    [Header("Etc.")]
    [SerializeField] protected Transform _objectsContainer;
    [SerializeField] protected bool _isOn = true;
    [SerializeField] protected GameObject _itemEntityPrefab;

    #endregion

    #region Properties

    protected float _overclock;
    protected float _spawnInterval;
    public float RealTimeInterval { get; private set; } = 5f;
    public float Performance { get; private set; } = 100f;
    public float SpawnInterval => _spawnInterval;
    public float Overclock => _overclock;
    public bool IsOn => _isOn;
    public IInventory Inventory => _inventory;

    [SerializeField] private GameObject _healthBarPrefab;
    private HealthBarUI _healthBar;

    #endregion

    private readonly Queue<float> _spawnTimes = new();
    private int _maxSpawnRecords = 5;
    private IInventory _inventory;

    private void Awake()
    {
        _inventory = GetComponent<IInventory>();
        if (_inventory == null && _recipe != null && _recipe.Ingredients.Length > 0)
            throw new Exception("Inventory component is required if recipe has ingredients");

        _overclock = stats.Overclock;
        _spawnInterval = stats.SpawnInterval;
    }

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
        if (_objectsContainer == null) _objectsContainer = GetComponentInParent<IZone>().ObjectsContainer;

        StartCoroutine(SpawnItemCoroutine());

        life = MaxLife;

        GameObject healthBarInstance = Instantiate(_healthBarPrefab, transform.position, Quaternion.identity, GameObject.Find("MainCanvas").transform);
        healthBarInstance.transform.SetSiblingIndex(0);
        _healthBar = healthBarInstance.GetComponent<HealthBarUI>();
        _healthBar.Setup(this, transform);
    }

    protected virtual IEnumerator SpawnItemCoroutine()
    {
        while (_isOn)
        {
            yield return new WaitForSeconds(_spawnInterval / _overclock);

            if (_recipe == null) continue;

            if (!IsObjectPresent() && _recipe.CanCraft(_inventory))
            {
                var offset = transform.rotation * Vector3.up;
                var go = Instantiate(_itemEntityPrefab, transform.position + offset, Quaternion.identity, _objectsContainer);
                var itemEntity = go.GetComponent<ItemEntity>();
                itemEntity.Stack = _recipe.Craft(_inventory);

                var currentTime = Time.time;
                if (_spawnTimes.Count >= _maxSpawnRecords)
                {
                    _spawnTimes.Dequeue();
                }
                _spawnTimes.Enqueue(currentTime);
            }

            Performance = CalculatePerformance();
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
        if (Input.GetMouseButtonDown(1) && !Player.Instance.DeleteBuildingMode)
        {
            var cam = Camera.main;
            if (!cam) return;

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

        var performance = (actualSpawns / expectedSpawns) * 100f;
        return Mathf.Clamp(performance, 0f, 100f);
    }

    protected bool isDead = false;
    protected int life;

    public int MaxLife => stats.MaxLife;

    public int Life => life;

    public bool IsDead => isDead;

    public int TakeDamage(DamageStats damage)
    {
        Debug.Log("Building life is" + life);
        if (life > 0)
            life -= damage.TotalDamage;

        if (life <= 0)
            Die();

        return life;
    }

    public virtual void Die()
    {
        if (isDead) return;
        isDead = true;
        TileManager.Instance.SetUnoccupied(transform.position);
        Destroy(_healthBar.gameObject);
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (_healthBar != null) Destroy(_healthBar.gameObject);
    }

    public int HealDamage(DamageStats damage)
    {
        throw new System.NotImplementedException();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
