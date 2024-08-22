using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    IWeapon Owner { get; }
    float Speed { get; }
    float LifeTime { get; }
    Collider2D Collider { get; }
    Rigidbody2D Rb { get; }

    void Init();
    void Travel();
    void Die();
    void SetOwner(IWeapon owner);
}
