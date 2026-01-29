using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TimerAndVelocity : MonoBehaviour
{
    private Rigidbody rb;    
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI Velo;

    public float Timer;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        Timer += Time.deltaTime;

        int minutes = Mathf.FloorToInt(Timer / 60f);
        int seconds = Mathf.FloorToInt(Timer % 60f);
        int milliseconds = Mathf.FloorToInt((Timer * 1000f) % 1000f);

        timerText.text = $"{minutes:00}:{seconds:00}.{milliseconds}";

        float speed = rb.linearVelocity.magnitude;
        Velo.text = $"V: {speed:F2}";
    }
}
