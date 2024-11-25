using System;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class DialogueUI : Singleton<DialogueUI>
{
    private static readonly int OpenAnim = Animator.StringToHash("Open");
    private static readonly int CloseAnim = Animator.StringToHash("Close");

    [SerializeField] private Animator _dialogueAnimator;
    [SerializeField] private TextMeshProUGUI _dialogueText;

    private bool _open;
    private bool _canClose;

    public bool Open
    {
        get => _open;
        set
        {
            if (_open == value) return;

            if(value) ShowDialogue();
            else HideDialogue();

            _canClose = false;
        }
    }

    private void Update()
    {
        if (_canClose && Open && Input.anyKeyDown)
        {
            Open = false;
            _canClose = false;
        }
    }

    public void ShowDialogue(string text)
    {
        _dialogueText.text = text;
        ShowDialogue();
    }

    public void ShowDialogue()
    {
        Player.Instance.InputsEnabled = false;
        _dialogueAnimator.SetTrigger(OpenAnim);
        _open = true;
        Invoke(nameof(EnableClosing), 1.0f);
    }

    public void HideDialogue()
    {
        _dialogueAnimator.SetTrigger(CloseAnim);
        Player.Instance.InputsEnabled = true;
        _open = false;
    }

    private void EnableClosing()
    {
        _canClose = true;
    }
}