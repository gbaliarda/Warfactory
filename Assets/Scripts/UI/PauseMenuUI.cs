using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : Singleton<PauseMenuUI>
{
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private UnityEngine.UI.Slider musicSlider;
    [SerializeField] private UnityEngine.UI.Slider sfxSlider;
    void Start()
    {
        CloseMenu();
        InitializeSliders();
    }
    
    private void InitializeSliders()
    {
        musicSlider.value = AudioManager.Instance.musicVolume;
        sfxSlider.value = AudioManager.Instance.sfxVolume;
        
        musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);
    }
    
    public void ShowOptions()
    {
        optionsPanel.SetActive(true);
    }

    public void HideOptions()
    {
        optionsPanel.SetActive(false);
    }
    public void CloseMenu()
    {
        gameObject.SetActive(false);
        optionsPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void OpenMenu()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void TogglePause()
    {
        Time.timeScale = (Time.timeScale + 1) % 2;
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
