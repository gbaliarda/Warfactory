using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageStats", menuName = "Stats/Damage", order = 0)]
public class DamageStats : ScriptableObject
{
    [SerializeField] private DamageStatsValues _damageStats;

    public int PhysicalDamage => _damageStats.PhysicalDamage;
    public int FireDamage => _damageStats.FireDamage;
    public int WaterDamage => _damageStats.WaterDamage;
    public int LightningDamage => _damageStats.LightningDamage;
    public int VoidDamage => _damageStats.VoidDamage;
    public int TotalDamage => PhysicalDamage + FireDamage + WaterDamage + LightningDamage + VoidDamage;

}

[System.Serializable]
public struct DamageStatsValues
{
    public int PhysicalDamage;
    public int FireDamage;
    public int WaterDamage;
    public int LightningDamage;
    public int VoidDamage;
}