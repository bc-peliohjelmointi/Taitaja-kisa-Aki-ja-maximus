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
<<<<<<< Updated upstream
        if (other.CompareTag("Default"))
=======
        Debug.Log("no leg");
        if (other.CompareTag("Ground"))
>>>>>>> Stashed changes
        {
            Debug.Log("Jump no leg");
            controller.OnHit(true,other,hitbox);
        }
    }
}