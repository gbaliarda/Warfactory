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
            Debug.Log("Item agregado al cofre: " + Item.ItemName);

            if (Item.StackAmount <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
