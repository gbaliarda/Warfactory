using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PotionStats", menuName = "Stats/Potion", order = 0)]
public class PotionStats : ActorStats
{
    [SerializeField] private PotionStatsValues _potionStatValues;
    public float PotionCooldown => _potionStatValues.PotionCooldown;
    public float PotionDuration => _potionStatValues.PotionDuration;
}

[System.Serializable]
public struct PotionStatsValues
{
    public float PotionCooldown;
    public float PotionDuration;
}