using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _sightRange, _attackRange;


    protected MoveController moveController;
    protected AttackController attackController;
    private bool _playerInSight, _playerInAttack;

    void Awake()
    {
        moveController = GetComponent<MoveController>();

        if (moveController != null) moveController.SetSpeed(stats.MovementSpeed);
    }

    new void Update()
    {
        if (isDead) return;
        base.Update();
        _playerInSight = Physics2D.OverlapCircle(transform.position, _sightRange, _playerLayer);
        _playerInAttack = Physics2D.OverlapCircle(transform.position, _attackRange, _playerLayer);

        
        if (!_playerInSight && !_playerInAttack) Patrol();
        if (_playerInSight && !_playerInAttack) Chase();
        if (_playerInSight && _playerInAttack) Attack();
    }

    private void Patrol()
    {
    }

    private void Chase()
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

    private void Attack()
    {
        /*moveController.Move(transform.position);*/
        if (attackController == null) return;
        if (Player.Instance.GetComponent<IDamageable>() != null && Player.Instance.GetComponent<IDamageable>().IsDead == true) return;

        /*attackController.Attack(Player.Instance.transform.position);*/
    }
}
