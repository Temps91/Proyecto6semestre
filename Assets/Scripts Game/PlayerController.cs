using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed;
    public GameObject prefabBullet;
    public Transform shootPoint;

    public void Update()
    {
        Move();
        ShootAndReload();
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
        if (Input.GetKeyDown(KeyCode.F))
        {
            Instantiate(prefabBullet, shootPoint);
        }
        if (Input.GetKey(KeyCode.R))
        {

        }
    }
}
