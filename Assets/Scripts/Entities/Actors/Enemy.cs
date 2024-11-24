using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    [SerializeField] protected LayerMask _targetLayer;
    [SerializeField] protected float _sightRange, _attackRange;

    [SerializeField] private LootTable _lootTable;
    [SerializeField] private GameObject _itemEntityPrefab;

    [SerializeField] private GameObject _cannon;

    protected MoveController moveController;
    protected AttackController attackController;
    private bool _targetInSight, _targetInAttack;

    [SerializeField] private float _disappearDelay = 2f; // Time to wait before destroying after disappearing

    public float SightRange => _sightRange;

    [SerializeField] private string onHitSound = "enemyHit";

    protected override void Start()
    {
        if (TemporalLevel.Instance != null)
        {
            _runtimeStats.BoostStats(TemporalLevel.Instance.Difficulty / 5);
        }
        base.Start();

        moveController = GetComponent<MoveController>();
        if (moveController != null) moveController.SetSpeed(_runtimeStats.MovementSpeed);
    }

    protected override void Update()
    {
        if (isDead) return;

        base.Update();

        _targetInSight = Physics2D.OverlapCircle(transform.position, _sightRange, _targetLayer);
        _targetInAttack = Physics2D.OverlapCircle(transform.position, _attackRange, _targetLayer);

        
        if (!_targetInSight && !_targetInAttack) Patrol();
        if (_targetInSight && !_targetInAttack) Chase();
        if (_targetInSight && _targetInAttack) Attack();
    }

    protected virtual void Patrol()
    {
    }

    protected virtual void Chase()
    {
        if (Player.Instance.GetComponent<IDamageable>() != null && Player.Instance.GetComponent<IDamageable>().IsDead == true) return;
        if (moveController != null)
        {
            Vector2 direction = (Player.Instance.transform.position - transform.position).normalized;
            moveController.Move(direction);
            /*GetComponent<Rigidbody2D>().MovePosition(transform.position + Stats.MovementSpeed * 5 * Time.deltaTime * (Vector3)direction);*/
            /*moveController.Move(Player.Instance.transform.position);*/
        }
    }

    protected virtual void Attack()
    {
        /*moveController.Move(transform.position);*/
        if (attackController == null) return;
        if (Player.Instance.GetComponent<IDamageable>() != null && Player.Instance.GetComponent<IDamageable>().IsDead == true) return;

        /*attackController.Attack(Player.Instance.transform.position);*/
    }

    public override int TakeDamage(DamageStats damage)
    {
        if (life > 0)
        {
            life -= damage.TotalDamage;
            if (AudioManager.Instance != null && onHitSound != null)
                AudioManager.Instance.PlaySFX(onHitSound);
            if (animator != null)
            {
                animator.SetTrigger("Hit");
            }
        }

        if (life <= 0)
            Die();

        return life;
    }


    public override void Die()
    {
        if (isDead) return;

        isDead = true;

        // Trigger death animation
        if (animator != null)
        {
            animator.SetBool("Dead", true);
        }

        // Disable components
        if (moveController != null) moveController.enabled = false;
        if (GetComponent<Collider2D>() != null) GetComponent<Collider2D>().enabled = false;

        // Start coroutine to handle disappearing and item dropping
        StartCoroutine(DisappearAndDrop());
    }

    private IEnumerator DisappearAndDrop()
    {
        // Wait for the death animation to finish
        yield return new WaitForSeconds(deathAnimationDuration);

        SetVisibility(false);
        DropLoot();

        yield return new WaitForSeconds(_disappearDelay);

        Destroy(gameObject);
    }

    private void SetVisibility(bool isVisible)
    {
        // Disable all renderers on this object and its children
        SetRendererVisibility(this.gameObject, isVisible);
    }

    private void SetRendererVisibility(GameObject obj, bool isVisible)
    {
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = isVisible;
        }

        foreach (Transform child in obj.transform)
        {
            SetRendererVisibility(child.gameObject, isVisible);
        }
    }

    private void DropLoot()
    {
        var loot = _lootTable.GetLoot();
        foreach (var stack in loot)
        {
            var xOffset = Random.value - 0.5f;
            var yOffset = Random.value - 0.5f;
            var posOffset = new Vector3(xOffset, yOffset, 0);

            var drop = Instantiate(_itemEntityPrefab, transform.position + posOffset, Quaternion.identity);
            drop.GetComponent<ItemEntity>().Stack = stack;
        }
    }
}
