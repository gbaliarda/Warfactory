using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour, IProjectile
{
    public IWeapon Owner => owner;

    public float Speed => speed;

    public float LifeTime => lifetime;

    public Collider2D Collider => col;

    public Rigidbody2D Rb => rb;

    protected IWeapon owner;
    [SerializeField] protected float speed = 5;
    [SerializeField] protected float lifetime = 3;
    protected Collider2D col;
    protected Rigidbody2D rb;
    [SerializeField] protected LayerMask hittableMask;

    void Start()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        gameObject.layer = LayerMask.NameToLayer("Projectile");
        Init();
    }

    void Update()
    {
        Travel();

        lifetime -= Time.deltaTime;
        if (lifetime <= 0) Die();
    }

    public void SetOwner(IWeapon owner) => this.owner = owner;

    public void Init()
    {
        GetComponent<Collider2D>().isTrigger = true;
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

    }

    public void Travel()
    {
        EventQueueManager.Instance.AddEvent(new MoveTowardsDirectionCommand(transform, transform.right, Owner.Stats.Speed));
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (owner.Owner.CompareTag(other.tag)) return;

        if (((1 << other.gameObject.layer) & hittableMask) != 0)
        {
            if (other.GetComponent<IDamageable>() != null)
            {
                Debug.Log("Hit trigger");
                EventQueueManager.Instance.AddEvent(new ApplyDamageCommand(other.GetComponent<IDamageable>(), owner.Stats.DamageStats));
                if (other.GetComponent<Actor>().IsDead) return;
            }
            Die();
        }
    }
}
