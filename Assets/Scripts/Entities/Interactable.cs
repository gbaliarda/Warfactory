using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Interactable : MonoBehaviour
{
    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public abstract void Interact();
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player.Instance.OpenInteractableIcon();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player.Instance.CloseInteractableIcon();
        }
    }
}
