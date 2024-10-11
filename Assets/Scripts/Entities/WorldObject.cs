using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour
{
    public Item Item { get; set; }
    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Debug.Log("Pick up item");
                EventManager.Instance.EventPickUpWorldObject(this);
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ChestBuilding chest = collision.gameObject.GetComponent<ChestBuilding>();
        if (chest != null && Item != null)
        {
            Item = chest.AddItem(Item);

            if (Item.StackAmount <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
