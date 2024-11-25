using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : Singleton<TimerUI>
{
    [SerializeField] private TextMeshProUGUI _text;

    private void Start()
    {
        _text.text = "00:00";

        HideTimer();
    }

    public void SetTime(float seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60f);
        int remainingSeconds = Mathf.FloorToInt(seconds % 60f);

        _text.text = $"{minutes:00}:{remainingSeconds:00}";
    }
    public void HideTimer()
    {
        gameObject.SetActive(false);
    }

    public void ShowTimer()
    {
        gameObject.SetActive(true);
    }
}
