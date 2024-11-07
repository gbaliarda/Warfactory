using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponStats _stats;
    [SerializeField] private GameObject _prefabToInstantiate;
    [SerializeField] private Transform _bulletOrigin;

    private Actor _owner;
    protected float cooldownLeft;

    public GameObject PrefabToInstantiate => _prefabToInstantiate;
    public WeaponStats Stats => _stats;
    public Transform BulletOrigin => _bulletOrigin;
    public Actor Owner => _owner;
    public float CooldownLeft => cooldownLeft;

    public void Attack(Vector2 origin, Vector2 direction)
    {
        if (Owner.IsDead) return;
        if (cooldownLeft > 0) return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        GameObject projectile = Instantiate(_prefabToInstantiate, origin, rotation);
        projectile.GetComponent<Projectile>().SetOwner(this);

        if (_owner.Stats.BonusAttackSpeed > 0)
            cooldownLeft = _stats.Cooldown / _owner.Stats.BonusAttackSpeed;
        else
            cooldownLeft = _stats.Cooldown;
    }

    public void Attack(Vector2 direction)
    {
        Attack(_bulletOrigin.position, direction);
    }

    public void Attack()
    {
        Attack(transform.right);
    }

    protected void Start()
    {
        _owner = GetComponentInParent<Actor>();
        cooldownLeft = 0;
    }

    protected void Update()
    {
        if (cooldownLeft > 0) 
            cooldownLeft -= Time.deltaTime;
    }
}