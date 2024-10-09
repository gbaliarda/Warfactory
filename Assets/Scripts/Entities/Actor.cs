using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour, IDamageable
{
    public ActorStats Stats => stats;

    public int MaxLife => stats.MaxLife;

    public int Life => life;

    public bool IsDead => isDead;

    [SerializeField] protected ActorStats stats;

    protected bool isDead = false;
    protected int life;

    protected virtual void Start()
    {
        life = MaxLife;    
    }

    protected virtual void Update()
    {
        
    }

    public int TakeDamage(DamageStats damage)
    {
        Debug.Log("TakingDamage");
        if (life > 0)
            life -= damage.TotalDamage;

        if (life <= 0)
            Die();

        return life;
    }

    public virtual void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }

    public int HealDamage(DamageStats damage)
    {
        throw new System.NotImplementedException();
    }
}
