using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private string firstLevelScene = "level1";
    [SerializeField] private string menuMusicTrack = "theme";

    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
        optionsButton.onClick.AddListener(OpenOptions);
        quitButton.onClick.AddListener(QuitGame);
        
        AudioManager.Instance.SetMusicVolume(0.05f);
        AudioManager.Instance.SetSFXVolume(0.05f);
        AudioManager.Instance.PlayMusic(menuMusicTrack);
    }

    private void StartGame()
    {
        AudioManager.Instance.StopMusic();
        SceneManager.LoadScene(firstLevelScene);
    }

    private void OpenOptions()
    {
        // TODO: Implementar apertura del menú de opciones
        Debug.Log("Menú de opciones abierto");
    }

    private void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}