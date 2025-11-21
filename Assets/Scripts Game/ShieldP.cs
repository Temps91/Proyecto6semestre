using UnityEngine;
using UnityEngine.UI;

public class ShieldP : MonoBehaviour
{
    public GameObject InteractableShield;
    public PlayerController controller;
    public GameManager p;
    public bool playerEnter;
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
        if (playerEnter && Input.GetKey(KeyCode.F) && p.points >= 500)
        {
            Debug.Log("Comprando escudo");
            p.points -= 500;
            controller.shieldPerk = true;
        }
    }
}

