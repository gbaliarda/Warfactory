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
    [SerializeField] protected float deathAnimationDuration = 1f;
    [SerializeField] private GameObject _healthBarPrefab;
    private HealthBarUI _healthBar;

    protected bool isDead = false;
    protected int life;
    protected Animator animator;
    
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }
    protected virtual void Start()
    {
        life = MaxLife;

        GameObject healthBarInstance = Instantiate(_healthBarPrefab, transform.position, Quaternion.identity, FindObjectOfType<Canvas>().transform);
        healthBarInstance.transform.SetSiblingIndex(0);
        _healthBar = healthBarInstance.GetComponent<HealthBarUI>();
        _healthBar.Setup(this, transform);
    }

    protected virtual void Update()
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

    public virtual void Die()
    {
        if (isDead) return;
        isDead = true;
        if (animator != null) animator.SetBool("Dead", true);

        if (GetComponent<Collider2D>() != null) GetComponent<Collider2D>().enabled = false;
        
        StartCoroutine(DestroyAfterAnimation());
    }

    protected virtual IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(deathAnimationDuration);

        // Override this in derived classes if you need to do something before destruction
        // For example, Enemy class can override to spawn drops
        Destroy(_healthBar.gameObject);
        Destroy(gameObject);
    }

    public int HealDamage(DamageStats damage)
    {
        throw new System.NotImplementedException();
    }
}
