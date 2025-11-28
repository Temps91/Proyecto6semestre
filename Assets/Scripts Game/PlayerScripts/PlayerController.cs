using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 moveDirection; 
    
    private Coroutine regenRoutine;
    private Coroutine shieldRoutine;

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
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("PlayerController requiere un componente Rigidbody en el GameObject.");
        }
        
        juggerNog = false;
        StaminUp = false;
        shieldPerk = false;
        activeShield = false;
        speedCurrent = speed; 
        
        healthMax = health; 
        healthCurrent = health; 
    }

    public void Update()
    {
        HandleInput();
        
        ShieldTime();
        RegenerateHealth();
        
        MeleeKnife();
        Apuntar();
        LostPlayer();
        ShieldRevive();
        JuggerNog();
        
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
    
    private void FixedUpdate()
    {
        // 🟢 APLICAMOS MOVIMIENTO CON RIGIDBODY.VELOCITY
        if (rb == null) return;
        
        if (moveDirection.magnitude > 0)
        {
            Vector3 finalVelocity = moveDirection * speed;
            
            // 🛑 CORREGIDO: Usar rb.velocity.y
            finalVelocity.y = rb.linearVelocity.y; 

            // 🛑 CORREGIDO: Usar rb.velocity
            rb.linearVelocity = finalVelocity;
        }
        else
        {
             // Si no hay input, detenemos el movimiento horizontal
             // 🛑 CORREGIDO: Usar rb.velocity
             rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
    }
    
    public void HandleInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        
        // Calculamos la dirección relativa a la rotación del jugador
        moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        
        if (moveDirection.magnitude > 1)
        {
            moveDirection.Normalize();
        }
    }

    public void Move()
    {
        // Movimiento gestionado por FixedUpdate/Rigidbody.
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
        
        if (regenRoutine != null)
        {
            StopCoroutine(regenRoutine);
            regenRoutine = null;
            Debug.Log("Regeneración interrumpida por daño.");
        }
        
        if (healthCurrent < 0) healthCurrent = 0;
        LostPlayer();
    }
    
    public void JuggerNog()
    {
        if (juggerNog)
        {
            juggerNogImage.gameObject.SetActive(true);
            healthMax = healthWithJugger;
        }
        else if (!juggerNog)
        {
            juggerNogImage.gameObject.SetActive(false);
            healthMax = health;
            if (healthCurrent > healthMax)
            {
                healthCurrent = healthMax;
            }
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
    
    public void ShieldTime()
    {
        if (healthCurrent <= shieldMin && shieldPerk && shieldRoutine == null && !activeShield)
        {
            Debug.Log("Activando escudo");
            shieldRoutine = StartCoroutine(Invulnerability());
        }
    }

    IEnumerator Invulnerability()
    {
        activeShield = true;
        healthCurrent += healthShield; 
        shieldRevive.gameObject.SetActive(true); 

        yield return new WaitForSeconds(4);

        activeShield = false;
        shieldRevive.gameObject.SetActive(false); 
        
        if (healthCurrent > healthMax)
        {
             healthCurrent = healthMax;
        }
        
        Debug.Log("Escudo desactivado");
        shieldRoutine = null; 
    }
    
    public void RegenerateHealth()
    {
        if (healthCurrent < healthMax && regenRoutine == null)
        {
            Debug.Log("Vida es menor que la vida maxima, iniciando regeneración.");
            regenRoutine = StartCoroutine(RegenerateHealthCoroutine());
        }
    }

    IEnumerator RegenerateHealthCoroutine()
    {
        yield return new WaitForSeconds(3);
        
        if (healthCurrent < healthMax) 
        {
            healthCurrent = healthMax;
            Debug.Log("La vida de player es de " +  healthCurrent);
        }
        
        regenRoutine = null; 
    }
}