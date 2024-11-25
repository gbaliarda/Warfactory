using UnityEngine;

public class SignPost : MonoBehaviour, IInteractable
{
    [TextArea]
    [SerializeField]
    private string _signText = "Texto del cartel...";

    public void Interact()
    {
        DialogueUI.Instance.ShowDialogue(_signText);
    }
}
