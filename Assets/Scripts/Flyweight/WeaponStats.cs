using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStats", menuName = "Stats/Weapon", order = 0)]
public class WeaponStats : ScriptableObject
{
    [SerializeField] private WeaponStatsValues _weaponStats;

    public DamageStats DamageStats => _weaponStats._damageStats;
    public float Cooldown => _weaponStats._cooldown;
    public int Projectiles => _weaponStats._projectiles;
    public float Speed => _weaponStats._speed;
}

[System.Serializable]
public struct WeaponStatsValues
{
    public DamageStats _damageStats;
    public float _cooldown;
    public int _projectiles;
    public float _speed;
}
