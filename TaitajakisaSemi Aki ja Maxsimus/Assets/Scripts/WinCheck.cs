using UnityEngine;

public class WinCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        public GameObject screen;

        if (!other.CompareTag("Win"))
            return;

        TimerAndVelocity timer = other.GetComponent<TimerAndVelocity>();

        if (timer != null)
        {
            timer.FinishRun();
            Debug.Log("Highscore saved: " + timer.Timer);
        }
    }
}
