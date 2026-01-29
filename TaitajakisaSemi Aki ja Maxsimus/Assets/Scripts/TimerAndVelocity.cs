using UnityEngine;

public class TimerAndVelocity : MonoBehaviour
{
    private Rigidbody rb;    
    public TextMeshProUGUI Tier;
    public TextMeshProUGUI Velo;

    public float timerDuration;

    void awake()
    {
        rb = GetComponent<Rigidbody>();





    }
    void Update()
    {
        timerDuration += Time.deltaTime;

        
    }
}
