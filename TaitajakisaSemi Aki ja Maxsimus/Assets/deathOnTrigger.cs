using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class deathOnTrigger : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(1);
    }
}
