using TMPro;
using UnityEngine;

public class Doors : MonoBehaviour
{
    public int pointsCost;
    public bool playerEnter;
    public TextMeshProUGUI textDoor;
    public TextMeshProUGUI textPointsCost;
    public GameManager p;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerEnter = true;
            textDoor.gameObject.SetActive(true);
            textPointsCost.text = pointsCost.ToString();


        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerEnter = false;
            textDoor.gameObject.SetActive(false);
        }
    }

    public void Update()
    {

        if (playerEnter && Input.GetKey(KeyCode.F) && p.points >= pointsCost)
        {
            Debug.Log("Comprando escudo");
            p.points -= pointsCost;
            this.gameObject.SetActive(false);
        }
    }

}
