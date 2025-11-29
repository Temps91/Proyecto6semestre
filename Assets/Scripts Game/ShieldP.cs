using UnityEngine;
using UnityEngine.UI;

public class ShieldP : MonoBehaviour
{
    public GameObject InteractableShield;
    public PlayerController controller;
    public GameManager p;
    public bool playerEnter;
    bool interact;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerEnter = true;
            InteractableShield.SetActive(true);

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerEnter = false;
            InteractableShield.SetActive(false);
        }
    }


    public void Update()
    {
        interact = SimpleInput.GetButtonDown("Interact");
        if (playerEnter && interact && p.points >= 500)
        {
            Debug.Log("Comprando escudo");
            p.points -= 500;
            controller.shieldPerk = true;
            playerEnter = false;
        }
    }
}

