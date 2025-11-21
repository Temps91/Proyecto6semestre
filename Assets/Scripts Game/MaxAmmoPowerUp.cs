using System.Collections;
using UnityEngine;

public class MaxAmmoPowerUp : MonoBehaviour
{
    private float speed = 20f;
    public PlayerShooting pShoot;

    public void Start()
    {
        pShoot = FindAnyObjectByType<PlayerShooting>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("AgarreMAxAmo");
        pShoot.maxAmmo = true;
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
        StartCoroutine(TimePowerUp());
    }

    IEnumerator TimePowerUp()
    {
        yield return new WaitForSeconds(20f);
        this.gameObject.SetActive(false);
    }
}
