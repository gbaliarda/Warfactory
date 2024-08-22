using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyDamageCommand : ICommand
{
    private IDamageable _damageble;
    private DamageStats _damage;

    public ApplyDamageCommand(IDamageable damageble, DamageStats damage)
    {
        _damageble = damageble;
        _damage = damage;
    }

    public void Execute()
    {
        _damageble.TakeDamage(_damage);
    }

    public void Undo()
    {
        _damageble.HealDamage(_damage);
    }
}
