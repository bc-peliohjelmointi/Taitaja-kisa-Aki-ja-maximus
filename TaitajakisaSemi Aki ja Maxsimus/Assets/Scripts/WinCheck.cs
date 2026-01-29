using UnityEngine;

public class WinCheck : MonoBehaviour
{
    private bool won = false;
    public GameObject quit;

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
}
