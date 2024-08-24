using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActorStats", menuName = "Stats/Actor", order = 0)]
public class ActorStats : ScriptableObject
{
    [SerializeField] private ActorStatValues _actorStatValues;

    public int MaxLife => _actorStatValues.MaxLife;
    public float MovementSpeed => _actorStatValues.MovementSpeed;
    public int ProjectileIncrease => _actorStatValues.ProjectileIncrease;
    public float BonusAttackSpeed => _actorStatValues.BonusAttackSpeed;

    public void AddStats(ActorStats stats)
    {
        _actorStatValues.MaxLife += stats.MaxLife;
        _actorStatValues.MovementSpeed += stats.MovementSpeed;
        _actorStatValues.ProjectileIncrease += stats.ProjectileIncrease;
        _actorStatValues.BonusAttackSpeed += stats.BonusAttackSpeed;
    }
    
    public void RemoveStats(ActorStats stats)
    {
        _actorStatValues.MaxLife -= stats.MaxLife;
        _actorStatValues.MovementSpeed -= stats.MovementSpeed;
        _actorStatValues.ProjectileIncrease -= stats.ProjectileIncrease;
        _actorStatValues.BonusAttackSpeed -= stats.BonusAttackSpeed;
    }
}

[System.Serializable]
public struct ActorStatValues
{
    public int MaxLife;
    public float MovementSpeed;
    public int ProjectileIncrease;
    public float BonusAttackSpeed;
}