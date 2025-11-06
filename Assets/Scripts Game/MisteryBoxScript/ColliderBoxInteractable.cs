using UnityEngine;

public class ColliderBoxInteractable : MonoBehaviour
{
    public MisteryBoxController controllerBox;
    public bool playerInRangeBox;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRangeBox = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRangeBox = false;
        }
    }

    private void Update()
    {
        if (playerInRangeBox && Input.GetKey(KeyCode.F))
        {
            controllerBox.OpenMisteryBox();
        }
    }
}
