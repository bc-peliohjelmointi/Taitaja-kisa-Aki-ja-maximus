using UnityEngine;

public class Colliderhitboxscript : MonoBehaviour
{
    private Collider hitbox;
    PlayerController controller;
    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {


        Debug.Log("no leg");
        if (other.CompareTag("Ground"))
        {
            Debug.Log("Jump no leg");
        }
    }
}