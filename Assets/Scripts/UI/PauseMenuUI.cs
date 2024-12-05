using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : Singleton<PauseMenuUI>
{
    void Start()
    {
        CloseMenu();
    }
    public void CloseMenu()
    {
        gameObject.SetActive(false);
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
