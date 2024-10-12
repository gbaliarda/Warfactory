using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Slider _healthBarSlider;
    
    private IDamageable _damageable;

    private Transform _target;
    private Vector3 _offset = new Vector3(0.07f, -0.53f, 0);

    public void Setup(IDamageable damageable, Transform target)
    {
        _damageable = damageable;
        _target = target;
    }

    void Update()
    {
        if (_damageable == null) return;

        transform.position = Camera.main.WorldToScreenPoint(_target.position + _offset);

        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float healthPercent = (float)_damageable.Life / _damageable.MaxLife;
        _healthBarSlider.value = healthPercent;
    }
}
