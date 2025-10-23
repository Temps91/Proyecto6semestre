using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Velocidad del jugador")]
    public float speed;
    public float speedInSprint;
    [Header("Cuchillazo del jugador")]
    public GameObject melee;
    private bool meleeOn;
    [Header("Camaras")]
    public Camera camApuntar;
    public Camera camMain;

    public void Update()
    {
        Move();
        Sprint();
        MeleeKnife();
        Apuntar();


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

    public void Apuntar()
    {
        if (Input.GetMouseButton(1))
        {
            camMain.enabled = false;
            camApuntar.enabled = true;
        }
        else
        {
            camMain.enabled = true;
            camApuntar.enabled = false;
        }
    }

}
