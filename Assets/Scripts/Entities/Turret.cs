using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Actor, IDestroyable
{
    [SerializeField] protected LayerMask _targetLayer;
    [SerializeField] protected float _sightRange, _attackRange;
    [SerializeField] protected TurretCannon _cannon;
    protected AttackController attackController;
    private Enemy _targetInSight;


    public IInventory Inventory => _inventory;

    private IInventory _inventory;

    public void Destroy()
    {
        Destroy(gameObject);
    }

    protected override void Awake()
    {
        if (!TryGetComponent(out _inventory))
            throw new Exception("Inventory component is required");

        base.Awake();
    }

    protected override void Update()
    {
        if (isDead) return;

        base.Update();


        Collider2D[] enemiesInSight = Physics2D.OverlapCircleAll(transform.position, _sightRange, _targetLayer);
        _targetInSight = GetNearestEnemy(enemiesInSight);

        if (_targetInSight != null && Vector2.Distance(transform.position, _targetInSight.transform.position) <= _attackRange)
        {
            Attack(_targetInSight);
        }
        else
        {
            Patrol();
        }
    }

    protected void Patrol()
    {
        _cannon.transform.Rotate(0, 0, 30 * Time.deltaTime);
    }

    Enemy GetNearestEnemy(Collider2D[] enemies)
    {
        Enemy nearestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (var collider in enemies)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null && !enemy.IsDead)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEnemy = enemy;
                }
            }
        }

        return nearestEnemy;
    }

    void Attack(Enemy enemy)
    {
        if (enemy == null || enemy.IsDead) return;

        FaceTarget(enemy.transform);

        if (_cannon == null || _cannon is not IWeapon)
        {
            Debug.LogError("Weapon missing", this);
            return;
        }

        Vector2 dir = (enemy.transform.position - transform.position).normalized;
        _cannon.Attack(dir);
    }

    protected void FaceTarget(Transform target)
    {
        if (_cannon == null || target == null) return;

        Vector2 direction = (target.position - _cannon.transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        _cannon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void OnValidate()
    {
        if (_cannon == null || _cannon is not IWeapon)
            Debug.LogError("Weapon missing", this);
    }
}
