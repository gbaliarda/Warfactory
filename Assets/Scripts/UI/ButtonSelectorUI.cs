using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelectorUI : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    
    private Color selectedColor;
    private Color deselectedColor;

    private Button _selectedButton;

    public Button SelectedButton => _selectedButton;

    private void Start()
    {
        selectedColor = HexToColor("#C8854C");
        deselectedColor = HexToColor("#703F20");

        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => TrySelectButton(button));
        }
        SelectButton(buttons[0]);
    }

    private void TrySelectButton(Button selectedButton)
    {
        Debug.Log(selectedButton.interactable);
        if (!selectedButton.interactable) return;
        
        SelectButton(selectedButton);
    }

    private void SelectButton(Button selectedButton)
    {
        foreach (Button button in buttons)
        {
            var colors = button.colors;
            if (button == selectedButton)
            {
                _selectedButton = button;
                colors.normalColor = selectedColor;
            }
            else
            {
                colors.normalColor = deselectedColor;
            }
            button.colors = colors;
        }
    }

    private Color HexToColor(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out var color))
        {
            return color;
        }
        else
        {
            Debug.LogWarning($"Formato de color hexadecimal inválido: {hex}");
            return Color.white;
        }
    }
}
