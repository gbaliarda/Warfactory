using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class ItemEntity : MonoBehaviour
{
    [SerializeField] private Item _item;
    [SerializeField] private int _amount;

    public ItemStack Stack
    {
        get => _stack;
        set
        {
            _stack = value;
            _item = _stack.Item;
            _amount = _stack.Amount;
            _spriteRenderer.sprite = _item.Sprite;
        }
    }

    private ItemStack _stack;

    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (!_item) throw new Exception("Item is null");

        Stack = new ItemStack(_item, _amount);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var cam = Camera.main;
            if(!cam) return;

            Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Debug.Log("Pick up item");
                EventManager.Instance.EventPickUpItemEntity(this);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _rb.velocity = Vector2.zero;
        _rb.angularVelocity = 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Pick up item");
            EventManager.Instance.EventPickUpItemEntity(this);
        }

        var inventory = collision.gameObject.GetComponentInParent<IInventory>();
        if (inventory == null || Stack == null) return;

        Stack = inventory.AddItemStack(Stack);
        Debug.Log("Item agregado al cofre: " + _item.Name);

        if (Stack.Amount <= 0)
            Destroy(gameObject);
    }
}