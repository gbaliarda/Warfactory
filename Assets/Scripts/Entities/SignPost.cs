using UnityEngine;

public class SignPost : Interactable
{
    [SerializeField] private string signText = "Texto del cartel...";

    public override void Interact()
    {
        DialogueUI.Instance.ShowDialogue(signText);
    }
}