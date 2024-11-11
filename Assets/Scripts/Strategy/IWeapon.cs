using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    GameObject PrefabToInstantiate { get; }
    WeaponStats Stats { get; }
    Actor Owner { get; }
    Transform BulletOrigin { get; }

    float CooldownLeft { get; }
    void Attack();
    void Attack(Vector2 direction);
    void Attack(Vector2 origin, Vector2 direction);
}
