using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    GameObject PrefabToInstantiate {  get; }
    WeaponStats Stats { get; }
    Actor Owner { get; }

    float CooldownLeft { get; }
    void Attack();
    void Attack(Vector2 direction);
}
