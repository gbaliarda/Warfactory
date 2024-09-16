using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _sightRange, _attackRange;

    [SerializeField] private GameObject _drop;
    [SerializeField] private Sprite _dropSprite;

    protected MoveController moveController;
    protected AttackController attackController;
    private bool _playerInSight, _playerInAttack;

    void Awake()
    {
        moveController = GetComponent<MoveController>();

        if (moveController != null) moveController.SetSpeed(stats.MovementSpeed);
    }

    protected override void Update()
    {
        if (isDead) return;
        
        base.Update();
        
        _playerInSight = Physics2D.OverlapCircle(transform.position, _sightRange, _playerLayer);
        _playerInAttack = Physics2D.OverlapCircle(transform.position, _attackRange, _playerLayer);

        
        if (!_playerInSight && !_playerInAttack) Patrol();
        if (_playerInSight && !_playerInAttack) Chase();
        if (_playerInSight && _playerInAttack) Attack();
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
            moveController.Move((Vector3)direction);
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

    public override void Die()
    {
        int randomNumber = Random.Range(1, 3);
        if (randomNumber == 1 && _drop != null)
        {
            GameObject bullet = Instantiate(_drop, transform.position + Vector3.up, Quaternion.identity);
            bullet.GetComponent<WorldObject>().Item = new ShotgunBullet(1, "Shotgun Bullet", _dropSprite, 10, 5, ItemRarity.Common);
        }
        base.Die();
    }
}
