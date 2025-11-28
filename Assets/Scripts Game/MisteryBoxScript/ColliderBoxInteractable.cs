using TMPro;
using UnityEngine;

public class ColliderBoxInteractable : MonoBehaviour
{
    public TextMeshProUGUI textBox;
    public MisteryBoxController controllerBox;
    public bool playerInRangeBox;
    public int misteryPoints;
    public GameManager p;
    public Animator boxAnimator;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRangeBox = true;
            textBox.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRangeBox = false;
            textBox.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInRangeBox && Input.GetKey(KeyCode.F) && p.points >= misteryPoints)
        {
            p.points -= misteryPoints;
            controllerBox.OpenMisteryBox();
            textBox.gameObject.SetActive(false);
        }
    }
}
