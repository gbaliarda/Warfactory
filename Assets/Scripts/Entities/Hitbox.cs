using System;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private DamageStats stats;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Player.Instance.TakeDamage(stats);
        }
    }
}
