using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour, IDamageable
{
    public ActorStats Stats => _stats;

    public int MaxLife => _stats.MaxLife;

    public int Life => throw new System.NotImplementedException();

    public bool IsDead => isDead;

    [SerializeField] private ActorStats _stats;

    protected bool isDead = false;
    protected int life;

    protected void Start()
    {
        life = MaxLife;    
    }

    protected void Update()
    {
        
    }

    public int TakeDamage(DamageStats damage)
    {
        if (life > 0)
            life -= damage.TotalDamage;

        if (life <= 0)
            Die();

        return life;
    }

    public void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }

    public int HealDamage(DamageStats damage)
    {
        throw new System.NotImplementedException();
    }
}
