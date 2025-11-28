using UnityEngine;

public class StaminUp : MonoBehaviour
{
    public GameObject InteractableStaminUp;
    public GameManager p;
    public PlayerController controller;
    public bool playerEnter;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerEnter = true;
            InteractableStaminUp.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerEnter = false;
            InteractableStaminUp?.SetActive(false);
        }
    }


    public void Update()
    {
        if (playerEnter && Input.GetKey(KeyCode.F) && p.points >= 2000)
        {
            p.points -= 2000;
            controller.StaminUp = true;
            playerEnter = false;
        }
    }
}
