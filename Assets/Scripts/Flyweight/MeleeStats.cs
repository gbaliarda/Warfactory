using UnityEngine;

[CreateAssetMenu(fileName = "MeleeStats", menuName = "Stats/Melee", order = 1)]
public class MeleeStats : ScriptableObject
{
    [SerializeField] private MeleeStatsValues _meleeStats;

    public DamageStats DamageStats => _meleeStats._damageStats;
    public float Cooldown => _meleeStats._cooldown;
}

[System.Serializable]
public struct MeleeStatsValues
{
    public DamageStats _damageStats;  // Reference to your DamageStats ScriptableObject
    public float _cooldown;
}