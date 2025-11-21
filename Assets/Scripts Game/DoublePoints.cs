using System.Collections;
using UnityEngine;

public class DoublePoints : MonoBehaviour
{
    private float speed = 20;
    public GameManager points;
    public void Start()
    {
        points = FindAnyObjectByType<GameManager>();
    }
    public void Update()
    {
        
        //transform.Rotate(Vector3.right, speed *  Time.deltaTime);
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
        StartCoroutine(TimeDouble());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.Instance != null)
        GameManager.Instance.ActivateDoublePoints();
        this.gameObject.SetActive(false);
    }

    IEnumerator TimeDouble()
    {
        yield return new WaitForSeconds(20f);
        this.gameObject.SetActive(false);
    }

}
