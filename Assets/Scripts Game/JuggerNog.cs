using UnityEngine;
using UnityEngine.UI;

public class JuggerNog : MonoBehaviour
{
    public GameObject interactableJuggerNog;
    public GameManager p;
    public PlayerController controller;
    public bool playerEnter;
    public bool canBuy;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerEnter = true;
            interactableJuggerNog.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerEnter = false;
            interactableJuggerNog.SetActive(false);
        }
    }


    public void Update()
    {
        if (playerEnter && Input.GetKeyUp(KeyCode.F) && p.points >= 2500)
        {
            p.points -= 2500;
            controller.JuggerNog();
            controller.juggerNog = true;
            playerEnter = false;
        }
    }
}
