using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField]
    private UnityAction _onInteract;

    public UnityAction OnInteract
    {
        get => _onInteract;
        set => _onInteract = value;
    }


    public void Interact()
    {
        _onInteract?.Invoke();
    }
}
