using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class TurretCannon : MonoBehaviour, IWeapon
{
    [SerializeField] private Item _bulletItem;

    public GameObject PrefabToInstantiate => _prefabToInstantiate;
    public WeaponStats Stats => _stats;
    public Transform BulletOrigin => _bulletOrigin;
    [SerializeField] private Turret _turret;

    public float CooldownLeft => cooldownLeft;

    public Actor Owner => _turret;

    protected float cooldownLeft;

    [SerializeField] private WeaponStats _stats;
    [SerializeField] private GameObject _prefabToInstantiate;
    [SerializeField] private Transform _bulletOrigin;

    public void Attack(Vector2 direction)
    {
        Attack(_bulletOrigin.position, direction);
    }

    public void Attack(Vector2 origin, Vector2 direction)
    {
        if (_turret.Inventory.GetItemAmount(_bulletItem) == 0) return;
        if (_turret.IsDead) return;
        if (cooldownLeft > 0) return;

        int numberOfProjectiles = _stats.Projectiles;
        float angleBetweenProjectiles = 5f;

        if (360 / angleBetweenProjectiles < numberOfProjectiles)
            numberOfProjectiles = Mathf.RoundToInt(360 / angleBetweenProjectiles);

        Vector2 mainDirection = direction.normalized;
        float mainAngle = Mathf.Atan2(mainDirection.y, mainDirection.x) * Mathf.Rad2Deg;

        float maxRotationAngle = angleBetweenProjectiles * (numberOfProjectiles - 1) / 2;

        for (float rotationAngle = -maxRotationAngle; rotationAngle <= maxRotationAngle; rotationAngle += angleBetweenProjectiles)
        {
            float currentAngle = mainAngle + rotationAngle;

            Quaternion projectileRotation = Quaternion.Euler(0, 0, currentAngle);

            GameObject projectile = Instantiate(_prefabToInstantiate, origin, projectileRotation);
            projectile.GetComponent<Projectile>().SetOwner(this);
        }

        cooldownLeft = _stats.Cooldown;

        _turret.Inventory.RemoveItem(_bulletItem, 1);
    }

    void Start()
    {
        if (_turret == null) _turret = GetComponentInParent<Turret>();
    }

    void Update()
    {
        if (cooldownLeft > 0) cooldownLeft -= Time.deltaTime;
    }

    public void Attack()
    {
        throw new System.NotImplementedException();
    }
}
