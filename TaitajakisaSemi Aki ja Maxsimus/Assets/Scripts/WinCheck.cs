using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
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
