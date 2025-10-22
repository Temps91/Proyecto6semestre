using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed;
    public float speedInSprint;
    public GameObject prefabBullet;
    public Transform shootPoint;
    public LayerMask enemies;
    public GameObject melee;
    private bool meleeOn;

    public void Update()
    {
        Move();
        ShootAndReload();
        Sprint();
        MeleeKnife();


    }
    public void Move()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 Izquierda = new Vector3(-1, 0, 0);
            transform.Translate(Izquierda * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 Derecha = new Vector3(1, 0, 0);
            transform.Translate(Derecha * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 Adelante = new Vector3(0, 0, 1);
            transform.Translate(Adelante * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 Atras = new Vector3(0, 0, -1);
            transform.Translate(Atras * speed * Time.deltaTime);
        }
    }

    public void ShootAndReload()
    {
        Ray ray = new Ray(shootPoint.position, shootPoint.forward);
        RaycastHit hit;
        if (Input.GetMouseButton(0))
        {
            Debug.Log("disparo");
            if (Physics.Raycast(ray, out hit, 10f, enemies))
            {
                Debug.Log("raycast lanzado");
            }
        }

        Debug.DrawRay(shootPoint.position, shootPoint.forward * 10f, Color.red);
        if (Input.GetKey(KeyCode.R))
        {

        }
    }
    public void Sprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed += speedInSprint;
        }
    }
    public void MeleeKnife()
    {
        if (Input.GetKeyDown(KeyCode.V) && !meleeOn)
        {
            melee.SetActive(true);
            meleeOn = true;
            StartCoroutine(Cuchillazo());
        }  
    }
    IEnumerator Cuchillazo()
    {
        yield return new WaitForSeconds(0.5f);
        melee.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        meleeOn = false;
    }

}
