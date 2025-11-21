using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Imagenes")]
    public Image shieldImage;
    public Image shieldRevive;
    public Image juggerNogImage;
    public Image staminUpImage;
    [Header("Velocidad del jugador")]
    public float speed;
    public float speedCurrent;
    public float speedInSprint;
    public float speedWithStaminUp;
    [Header("Cuchillazo del jugador")]
    public GameObject melee;
    private bool meleeOn;
    [Header("Camaras")]
    public Camera camApuntar;
    public Camera camMain;
    [Header("Vida")]
    public int health;
    public int healthWithJugger;
    public int healthCurrent;
    public int healthShield;
    public int healthMax;
    public int shieldMin;
    [Header("Points")]
    public int points;
    [Header("Perks")]
    public bool juggerNog;
    public bool StaminUp;
    public bool shieldPerk;
    public bool activeShield;


    private void Start()
    {
        juggerNog = false;
        StaminUp = false;
        shieldPerk = false;
        activeShield = false;
        healthMax = healthCurrent;
        healthCurrent = juggerNog ? healthWithJugger : health;
    }

    public void Update()
    {
        ShieldTime();
        Move();
        MeleeKnife();
        Apuntar();
        LostPlayer();
        RegenerateHealth();
        ShieldRevive();


        if (StaminUp)
        {
            speed = speedWithStaminUp;
            staminUpImage.gameObject.SetActive(true);
        }
        else if (!StaminUp)
        {
            speed = speedCurrent;
            staminUpImage.gameObject.SetActive(false);
        }

        Debug.Log("vida actual" + healthCurrent);




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

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Zone>() != null)
        {
            other.gameObject.GetComponent<Zone>().PlayerEntered();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Zone>() != null)
        {
            other.gameObject.GetComponent<Zone>().PlayerExited();
        }
    }

    public void LostPlayer()
    {
        if (healthCurrent <= 0)
        {
            Debug.Log("Moriste");
            this.gameObject.SetActive(false);
        }
    }

    public void TakeDamagePlayer(int amount)
    {
        Debug.Log("tomando daño");
        healthCurrent -= amount;
        if (healthCurrent < 0) healthCurrent = 0;
        LostPlayer();
        
    }
    public void JuggerNog()
    {
        if (juggerNog)
        {
            juggerNogImage.gameObject.SetActive(true);
            healthCurrent = healthWithJugger;
            healthMax = healthCurrent;
        }
        else if (!juggerNog)
        {
            juggerNogImage.gameObject.SetActive(false);
            healthCurrent = health;
            healthMax = healthCurrent;
        }
    }

    public void ShieldRevive()
    {
        if (shieldPerk)
        {
            shieldImage.gameObject.SetActive(true);
        }
        else if (!shieldPerk)
        {
            shieldImage.gameObject.SetActive(false);
        }
    }

    IEnumerator Invulnerability()
    {
        activeShield = true;
        healthCurrent += healthShield;
        yield return new WaitForSeconds(4);
        activeShield = false;
        healthCurrent = healthMax;
        Debug.Log("Escudo desactivado");
    }
    public void RegenerateHealth()
    {
        if (healthCurrent < healthMax)
        {
            Debug.Log("Vida es menor que la vida maxima");
            StartCoroutine(RegenerateHealthCoroutine());
        }
    }

    IEnumerator RegenerateHealthCoroutine()
    {
        yield return new WaitForSeconds(3);
        healthCurrent = healthMax;
        Debug.Log("la vida de player es de " +  healthCurrent);
    }

    public void ShieldTime()
    {
        if (healthCurrent <= shieldMin && shieldPerk)
        {
            Debug.Log("Activando escudo");
            StartCoroutine(Invulnerability());
        }
    }
}
