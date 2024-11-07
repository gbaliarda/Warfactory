using UnityEngine;

public class MeleeEnemy : Enemy
{
    private static readonly int AttackTrigger = Animator.StringToHash("Attack");
    [SerializeField] private MeleeStats _meleeStats;
    [SerializeField] private bool _facingRight = true;

    [Header("Slash Effect")]
    [SerializeField] private GameObject _slashPrefab;

    private float _nextAttackTime;

    private bool IsAttacking => animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");

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
        if (IsAttacking || _nextAttackTime > Time.time) return;

        // Face the player
        FaceTarget(Player.Instance.transform);

        animator.SetTrigger(AttackTrigger);
        

        if (stats.BonusAttackSpeed > 0)
            _nextAttackTime = Time.time + _meleeStats.Cooldown / stats.BonusAttackSpeed;
        else
            _nextAttackTime = Time.time + _meleeStats.Cooldown;
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