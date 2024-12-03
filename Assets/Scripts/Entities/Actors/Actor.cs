using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Actor : MonoBehaviour, IDamageable
{
    public ActorStats Stats => _runtimeStats;

    public int MaxLife => _runtimeStats.MaxLife;

    public int Life => life;

    public bool IsDead => isDead;

    [SerializeField] private ActorStats stats;
    [SerializeField] protected float deathAnimationDuration = 1f;
    [SerializeField] private GameObject _healthBarPrefab;
    private HealthBarUI _healthBar;

    protected bool isDead = false;
    protected int life;
    protected Animator animator;
    protected ActorStats _runtimeStats;

    protected bool _isMoving;
    public bool IsMoving => _isMoving;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();

        _runtimeStats = Instantiate(stats);

    }
    protected virtual void Start()
    {
        life = MaxLife;
        GameObject healthBarInstance = Instantiate(_healthBarPrefab, transform.position, Quaternion.identity, GameObject.Find("MainCanvas").transform);
        healthBarInstance.transform.SetSiblingIndex(0);
        _healthBar = healthBarInstance.GetComponent<HealthBarUI>();
        _healthBar.Setup(this, transform);
    }

    protected virtual void Update()
    {
        
    }

    public void SetIsMoving(bool isMoving)
    {
        _isMoving = isMoving;
    }

    private void FixedUpdate()
    {

        if(GetComponent<Rigidbody2D>() != null) GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public virtual int TakeDamage(DamageStats damage)
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

    public virtual int HealDamage(DamageStats damage)
    {
        throw new System.NotImplementedException();
    }

    private void OnDestroy()
    {
        if (_healthBar != null) Destroy(_healthBar.gameObject);
    }
}
