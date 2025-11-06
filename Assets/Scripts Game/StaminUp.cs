using UnityEngine;

public class StaminUp : MonoBehaviour
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
        if (playerEnter && Input.GetKey(KeyCode.F))
        {
            controller.StaminUp = true;
        }
    }
}
