using UnityEngine;

public class ColliderBoxInteractable : MonoBehaviour
{
    public MisteryBoxController controllerBox;
    public bool playerInRangeWeapon;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRangeWeapon = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRangeWeapon = false;
        }
    }

    private void Update()
    {
        if (playerInRangeWeapon && Input.GetKey(KeyCode.F))
        {
            controllerBox.OpenMisteryBox();
        }
    }
}
