using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public bool MainMenuEnable;

    public InputActionAsset Assets;

    public GameObject PauseMenu;

    private bool Paused = false;

    private InputAction _pause;
    private void OnEnable()
    {
        _pause = Assets.FindAction("UI/Pause");

        _pause.started += OnPause;
        _pause.Enable();
    }
    private void OnDisable()
    {
        _pause = Assets.FindAction("UI/Pause");

        _pause.started -= OnPause;
        _pause.Disable();
    }

    private void OnPause(InputAction.CallbackContext context)
    {

            if (!Paused)
            {
                Paused = true;
            }
            else
            {
                Paused = false;
            }




        OpenCloseMenu();
    }

    public void OpenCloseMenu()
    {
        PauseMenu.SetActive(Paused);

        if (!MainMenuEnable)
        {
            if (Paused)
            {
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        else
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void Start()
    {
        if (MainMenuEnable)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        PauseMenu.SetActive(false);
        OpenCloseMenu();
    }

    public void MainMenu(bool Mainmenu)
    {
        SceneManager.LoadScene(0);
    }

    public void Play(bool Play)
    {
        SceneManager.LoadScene(1);
    }

    public void Quit(bool Quit)
    {
        Application.Quit();
    }

    public void Resume(bool Resume)
    {
        Paused = false;

        OpenCloseMenu();
    }
}
