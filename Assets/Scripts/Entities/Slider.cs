using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Slider : MonoBehaviour, IDestroyable
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private Vector3 _direction;

    private HashSet<Rigidbody2D> _rigidBodyToMove = new();
    private bool _isMoving = false;

    public Vector3 Direction => _direction;

    private void Awake()
    {
        _direction = transform.rotation * Vector3.up;
    }

    private void Update()
    {
        if (_isMoving)
        {
            foreach (var rb in _rigidBodyToMove)
            {
                if (rb != null)
                {
                    rb.velocity = _direction * _moveSpeed;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.gameObject.activeInHierarchy)
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                _rigidBodyToMove.Add(rb);
                _isMoving = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other != null)
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
                _rigidBodyToMove.Remove(rb);

                if (_rigidBodyToMove.Count == 0)
                {
                    _isMoving = false;
                }
            }
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}