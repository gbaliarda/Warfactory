using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;

public class EnemyDestroyer : WeaponEnemy
{
    [SerializeField] protected LayerMask _buildingLayer;
    protected override void Attack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _sightRange, _buildingLayer);

        if (colliders.Length > 0)
        {
            Collider2D[] attackColliders = Physics2D.OverlapCircleAll(transform.position, _attackRange, _buildingLayer);
            if (attackColliders.Length == 0)
            {
                Chase();
                return;
            }
            Transform closestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = transform.position;

            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.GetComponent<IDamageable>() == null || collider.gameObject.GetComponent<IDamageable>().IsDead == true) continue;
                Vector3 directionToTarget = collider.transform.position - currentPosition;
                float distanceSqrToTarget = directionToTarget.sqrMagnitude;

                if (distanceSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqrToTarget;
                    closestTarget = collider.transform;
                }
            }

            if (closestTarget != null)
            {
                FaceTarget(closestTarget.transform);
                if (_weapon == null || _weapon is not IWeapon)
                {
                    Debug.LogError("Weapon missing", this);
                    return;
                }

                var dir = (closestTarget.transform.position - transform.position).normalized;
                Weapon.Attack(dir);
            }
        } else
        {
            base.Attack();
        }
    }

    protected override void Chase()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _sightRange, _buildingLayer);

        if (colliders.Length > 0)
        {
            Transform closestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = transform.position;

            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.GetComponent<IDamageable>() == null || collider.gameObject.GetComponent<IDamageable>().IsDead == true) continue;
                Vector3 directionToTarget = collider.transform.position - currentPosition;
                float distanceSqrToTarget = directionToTarget.sqrMagnitude;

                if (distanceSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqrToTarget;
                    closestTarget = collider.transform;
                }
            }

            if (closestTarget != null)
            {
                if (moveController != null)
                {
                    Vector2 direction = (closestTarget.transform.position - transform.position).normalized;
                    moveController.Move(direction);
                    FaceTarget(closestTarget.transform);
                }
            }
        } else
        {
            base.Chase();
        }
    }
}
