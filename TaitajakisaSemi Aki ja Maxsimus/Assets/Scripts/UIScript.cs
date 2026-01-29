using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public static class Score
{
    private const string HighScoreKey = "HighScore";

    public static float highScore;

    public static void Load()
    {
        highScore = PlayerPrefs.GetFloat(HighScoreKey, 0f);
    }

    public static void Set(float time)
    {
        if (highScore == 0f || time < highScore)
        {
            highScore = time;
            PlayerPrefs.SetFloat(HighScoreKey, highScore);
            PlayerPrefs.Save();
        }
    }
}

public class UIScript : MonoBehaviour
{
    public bool MainMenuEnable;

    public TextMeshProUGUI h1ghScore;

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
        Score.Load();

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
    void Update()
    {
        h1ghScore.text = $"Fastest Time: {Score.highScore}";
    }
}
