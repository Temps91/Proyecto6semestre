using UnityEngine;

public class ShieldP : MonoBehaviour
{
    public PlayerController controller;
    public bool playerEnter;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerEnter = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerEnter = false;
        }
    }


    public void Update()
    {
        if (playerEnter && Input.GetKey(KeyCode.F) && controller.points >= 500)
        {
            Debug.Log("Comprando escudo");
            controller.points -= 500;
            controller.shieldPerk = true;
        }
    }
}

