using UnityEngine;

public class MeleeEnemy : Enemy
{
    [SerializeField] private MeleeStats _meleeStats;
    [SerializeField] private bool _facingRight = true;
    [SerializeField] private Vector2 _attackOffset = new Vector2(1f, 0f);
    [SerializeField] private Vector2 _attackSize = new Vector2(1.5f, 1.5f);
    [SerializeField] private Color _attackFlashColor = Color.red;
    [SerializeField] private float _flashDuration = 0.1f;
    private float _cooldownLeft;
    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;

    protected override void Awake()
    {
        base.Awake();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer != null)
        {
            _originalColor = _spriteRenderer.color;
        }
    }

    protected override void Update()
    {
        base.Update();
        if (_cooldownLeft > 0) _cooldownLeft -= Time.deltaTime;
    }

    protected override void Attack()
    {
        var player = Player.Instance;
        if (player.IsDead) return;
        if (_cooldownLeft > 0) return;
        FaceTarget(player.transform);

        // Visual feedback for attack (flash)
        StartCoroutine(AttackFlash());

        // Calculate attack position based on facing direction
        Vector2 attackPos = (Vector2)transform.position +
                            (_facingRight ? _attackOffset : new Vector2(-_attackOffset.x, _attackOffset.y));

        // Check for hits using OverlapBox
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackPos, _attackSize, 0f, _targetLayer);
        foreach (Collider2D hit in hits)
        {
            if (hit.GetComponentInParent<IDamageable>() != null && !hit.CompareTag(tag))
            {
                DamageStats damageStats = _meleeStats.DamageStats;
                EventQueueManager.Instance.AddEvent(new ApplyDamageCommand(hit.GetComponentInParent<IDamageable>(),
                    damageStats));
            }
        }

        if (stats.BonusAttackSpeed > 0)
            _cooldownLeft = _meleeStats.Cooldown / stats.BonusAttackSpeed;
        else
            _cooldownLeft = _meleeStats.Cooldown;
    }

    private System.Collections.IEnumerator AttackFlash()
    {
        if (_spriteRenderer != null)
        {
            _spriteRenderer.color = _attackFlashColor;
            yield return new WaitForSeconds(_flashDuration);
            _spriteRenderer.color = _originalColor;
        }
    }

    protected override void Chase()
    {
        if (Player.Instance.IsDead) return;
        base.Chase();
        var player = Player.Instance;
        FaceTarget(player.transform);
    }

    protected void FaceTarget(Transform target)
    {
        if (target.position.x > transform.position.x && !_facingRight)
        {
            Flip();
        }
        else if (target.position.x < transform.position.x && _facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}