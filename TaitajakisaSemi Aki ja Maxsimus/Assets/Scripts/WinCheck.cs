using UnityEngine;
using TMPro;

public class WinCheck : MonoBehaviour
{
    private bool won = false;
    public GameObject quit;
    public TextMeshProUGUI LMFAO;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Win"))
            return;

        TimerAndVelocity timer = other.GetComponent<TimerAndVelocity>();

        if (timer != null)
        {
            timer.FinishRun();
            Debug.Log("Highscore saved: " + timer.Timer);
            won = true;

            Time.timeScale = 0f; 
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            quit.SetActive(true);

            Score.Load();

            UpdateHighScoreUI();
        }
    }

    void Update()
    {
        if (won)
        {
             Time.timeScale = 0f;
             Cursor.lockState = CursorLockMode.None;
             Cursor.visible = true;
        }
    }

    public void UpdateHighScoreUI()
    {
        float time = Score.highScore;

        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 1000f) % 1000f);

        LMFAO.text = $"Best Time: {minutes:00}:{seconds:00}.{milliseconds:000}";
    }
}
