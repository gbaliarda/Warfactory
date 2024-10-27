using UnityEngine;

public class MeleeEnemy : Enemy
{
    [SerializeField] private MeleeStats _meleeStats;
    [SerializeField] private bool _facingRight = true;
    [SerializeField] private Vector2 _attackOffset = new Vector2(1f, 0f);
    [SerializeField] private Vector2 _attackSize = new Vector2(1.5f, 1.5f);
    
    [Header("Slash Effect")]
    [SerializeField] private GameObject _slashPrefab;
    [SerializeField] private Vector2 _slashOffset = new Vector2(0.5f, 0f);
    [SerializeField] private Vector3 _slashRotation = new Vector3(0, 0, -45f);
    
    private float _cooldownLeft;
    private SpriteRenderer _spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Update()
    {
        base.Update(); // Let base class handle state transitions
        if (_cooldownLeft > 0) _cooldownLeft -= Time.deltaTime;
    }
    
    protected override void Chase()
    {
        if (Player.Instance.IsDead) return;
        base.Chase();
            
        var player = Player.Instance;
        FaceTarget(player.transform);
    }
    

    // Override the base Attack method
    protected override void Attack()
    {
        if (Player.Instance.IsDead) return;
        if (_cooldownLeft > 0) return;

        // Face the player
        FaceTarget(Player.Instance.transform);
        
        // Spawn slash effect
        if (_slashPrefab != null)
        {
            Vector3 slashPosition = transform.position + new Vector3(
                _facingRight ? _slashOffset.x : -_slashOffset.x,
                _slashOffset.y,
                0
            );
            
            GameObject slash = Instantiate(_slashPrefab, slashPosition, Quaternion.identity);
            
            Vector3 scale = slash.transform.localScale;
            scale.x = _facingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            slash.transform.localScale = scale;
            
            slash.transform.rotation = Quaternion.Euler(
                _slashRotation.x,
                _slashRotation.y,
                _facingRight ? _slashRotation.z : -_slashRotation.z
            );
        }
        
        Vector2 attackPos = (Vector2)transform.position +
                          (_facingRight ? _attackOffset : new Vector2(-_attackOffset.x, _attackOffset.y));

        Collider2D[] hits = Physics2D.OverlapBoxAll(attackPos, _attackSize, 0f, _targetLayer);
        foreach (Collider2D hit in hits)
        {
            if (hit.GetComponentInParent<IDamageable>() != null && !hit.CompareTag(tag))
            {
                DamageStats damageStats = _meleeStats.DamageStats;
                EventQueueManager.Instance.AddEvent(new ApplyDamageCommand(
                    hit.GetComponentInParent<IDamageable>(),
                    damageStats
                ));
            }
        }
        
        if (stats.BonusAttackSpeed > 0)
            _cooldownLeft = _meleeStats.Cooldown / stats.BonusAttackSpeed;
        else
            _cooldownLeft = _meleeStats.Cooldown;
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