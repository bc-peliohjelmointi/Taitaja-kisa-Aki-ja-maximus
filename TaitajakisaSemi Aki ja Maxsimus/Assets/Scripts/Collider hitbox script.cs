using UnityEngine;

public class Colliderhitboxscript : MonoBehaviour
{
    public Collider hitbox;

    PlayerController controller;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            controller.OnHit(true,other,hitbox);
        }
    }
}