using UnityEngine;

public class PlayerInteractable : MonoBehaviour
{
    private MisteryBoxController currentBox;
    private bool playerInside = false;

    private void OnTriggerEnter(Collider other)
    {
        MisteryBoxController box = other.GetComponent<MisteryBoxController>();
        if (box != null)
        {
            currentBox = box;
            playerInside = true;
            Debug.Log("Jugador cerca de la caja");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<MisteryBoxController>() != null)
        {
            playerInside = false;
            currentBox = null;
            Debug.Log("Jugador se alejó de la caja");
        }
    }

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.F))
        {
            if (currentBox != null && !currentBox.IsOpen)
            {
                currentBox.OpenMisteryBox();
                Debug.Log("Abriendo la caja");
            }
        }
    }
}
