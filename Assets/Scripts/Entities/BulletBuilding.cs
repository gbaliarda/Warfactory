using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBuilding : MonoBehaviour
{
    [SerializeField] private GameObject _bulletToFabricate;
    [SerializeField] private Transform _objectsContainer;
    [SerializeField] private Sprite _bulletSprite;
    [SerializeField] private float _spawnInterval = 5f;
    private float _realTimeInterval = 5f;
    private float _performance = 100f;
    [SerializeField] private float _overcloak = 1f;
    [SerializeField] private bool _isOn = true;

    public float SpawnInterval => _spawnInterval;
    public float RealTimeInterval => _realTimeInterval;
    public float Performance => _performance;
    public float OverCloak => _overcloak;
    public bool IsOn => _isOn;

    private Queue<float> _spawnTimes = new Queue<float>();
    private int _maxSpawnRecords = 5;

    public void SetOverCloak(float value)
    {
        _overcloak = value;
        if (_overcloak == 0) _realTimeInterval = 0;
        else _realTimeInterval = _spawnInterval / _overcloak;
    }
    
    public void SetIsOn(bool isOn)
    {
        _isOn = isOn;
        if (_isOn)
        {
            StartCoroutine(SpawnBulletCoroutine());
        }
    }

    void Start()
    {
        if (_objectsContainer == null) _objectsContainer = GameObject.Find("Objects").transform;

        StartCoroutine(SpawnBulletCoroutine());
    }

    private IEnumerator SpawnBulletCoroutine()
    {
        while (_isOn)
        {
            yield return new WaitForSeconds(_spawnInterval / _overcloak);

            if (!IsObjectPresent())
            {
                Vector3 offset = transform.rotation * Vector3.up;
                GameObject bullet = Instantiate(_bulletToFabricate, transform.position + offset, Quaternion.identity, _objectsContainer);
                bullet.GetComponent<WorldObject>().Item = new ShotgunBullet(1, "Shotgun Bullet", _bulletSprite, 10, ItemRarity.Common);
                
                float currentTime = Time.time;
                if (_spawnTimes.Count >= _maxSpawnRecords)
                {
                    _spawnTimes.Dequeue();
                }
                _spawnTimes.Enqueue(currentTime);
            }


            CalculatePerformance();
        }
    }

    private bool IsObjectPresent()
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

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            LayerMask buildingLayer = LayerMask.GetMask("Building");
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, buildingLayer);
            Debug.Log($"ASD {gameObject}");
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Debug.Log($"{hit.collider.gameObject}");
                Debug.Log("DEF");
                EventManager.Instance.EventOpenBuildingUI(this);
            }
        }
    }

    public float CalculatePerformance()
    {
        if (_spawnTimes.Count < 5) return 100f;

        float[] spawnTimesArray = _spawnTimes.ToArray();
        float firstTime = spawnTimesArray[0];
        float lastTime = spawnTimesArray[spawnTimesArray.Length - 1];
        float totalTime = lastTime - firstTime;

        float expectedSpawns = totalTime / (_spawnInterval / _overcloak) + 1;

        float actualSpawns = _spawnTimes.Count;

        _performance = (actualSpawns / expectedSpawns) * 100f;
        return Mathf.Clamp(_performance, 0f, 100f);
    }
}
