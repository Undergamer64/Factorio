using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseMenu;

    [SerializeField]
    private PlayerInput _playerInput;

    private void Awake()
    {
        Time.timeScale = 1.0f;
    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void Pause() 
    {
        _playerInput.SwitchCurrentActionMap("Pause");
        _pauseMenu.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void UnPause()
    {
        _playerInput.SwitchCurrentActionMap("InGame");
        Time.timeScale = 1.0f;
        _pauseMenu.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
