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

    public void BoostStats(int boost)
    {
        _actorStatValues.MaxLife *= 1 + boost;
        _actorStatValues.MovementSpeed *= 1 + boost / 2;
        _actorStatValues.ProjectileIncrease = Mathf.FloorToInt(_actorStatValues.ProjectileIncrease * (1 + boost));
        _actorStatValues.BonusAttackSpeed *= 1 + boost / 2;
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