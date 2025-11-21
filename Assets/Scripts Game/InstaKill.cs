using System.Collections;
using UnityEngine;

public class InstaKill : MonoBehaviour
{
    private float speed = 20f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnemyTimer timer = FindAnyObjectByType<EnemyTimer>();

            if (timer != null)
            {
                timer.ActivateInstaKill();
                this.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
        StartCoroutine(TimeInsta());
    }

    IEnumerator TimeInsta()
    {
        yield return new WaitForSeconds(20f);
        this.gameObject.SetActive(false);
    }
}
