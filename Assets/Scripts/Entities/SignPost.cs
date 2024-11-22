using UnityEngine;

public class SignPost : Interactable
{
    [SerializeField] private string signText = "Texto del cartel...";
    [SerializeField] private DialogueUI dialogueUI;

    public override void Interact()
    {
        dialogueUI.ShowDialogue(signText);
    }
}