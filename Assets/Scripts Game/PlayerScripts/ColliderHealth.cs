using System.Collections;
using UnityEngine;

public class ColliderHealth : MonoBehaviour
{
    private Coroutine attackRoutine; // Renombrado a attackRoutine para mayor claridad
    
    [Header("Configuración de Ataque")]
    // 🟢 attackRate: Tiempo total entre un ataque y el siguiente (incluyendo animación)
    public float attackRate = 1.5f; 
    // 🟢 hitDelay: Tiempo de espera DENTRO de la animación de ataque para infligir daño.
    // Ej: Si la animación dura 1.0s, el daño puede caer a 0.3s.
    public float hitDelay = 0.3f; 
    public int damageAmount = 1;

    // Componentes
    public PlayerController playerDamage;
    private Enemy enemy;
    
    // Estado
    private bool playerInside = false;

    private void Start()
    {
        // 🟢 Usar GetComponentInParent es más seguro para obtener componentes en la jerarquía
        playerDamage = FindAnyObjectByType<PlayerController>(); 
        enemy = GetComponentInParent<Enemy>();
        
        // Comprobación de errores
        if (playerDamage == null)
            Debug.LogError("ColliderHealth: No se encontró PlayerController.");
        if (enemy == null)
            Debug.LogError("ColliderHealth: No se encontró el script Enemy en el padre.");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            if (!playerInside) // Solo si acaba de entrar
            {
                playerInside = true;
                
                // 🟢 Solo iniciamos el ciclo de ataque si no está corriendo ya
                if (attackRoutine == null) 
                {
                    enemy?.StopMovement();
                    attackRoutine = StartCoroutine(AttackRoutine());
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            playerInside = false;

            // 🟢 Detenemos el ciclo de ataque si el jugador sale
            if (attackRoutine != null)
            {
                StopCoroutine(attackRoutine);
                attackRoutine = null;
            }

            // 🟢 Reanudamos el movimiento del enemigo
            enemy?.ResumeMovement();
        }
    }

    private IEnumerator AttackRoutine()
    {
        // El enemigo permanece quieto mientras el jugador está dentro
        enemy?.StopMovement(); 
        
        // 🟢 Bucle principal de ataque
        while (playerInside)
        {
            // 1. Dispara la animación (PlayAttack)
            enemy?.PlayAttack();

            // 2. Esperamos el tiempo necesario para que el golpe de la animación impacte (hitDelay)
            // Esto sincroniza el daño con el momento visual del golpe.
            yield return new WaitForSeconds(hitDelay); 
            
            // 3. Aplicar el daño
            if (playerInside && playerDamage != null) // Doble verificación por si el jugador muere o sale justo aquí
            {
                playerDamage.TakeDamagePlayer(damageAmount);
            }

            // 4. Esperamos el tiempo restante del attackRate
            // TiempoTotal = hitDelay + TiempoRestante
            // TiempoRestante = attackRate - hitDelay
            float remainingWaitTime = attackRate - hitDelay;
            
            if (remainingWaitTime > 0)
            {
                 yield return new WaitForSeconds(remainingWaitTime);
            }
            // Si remainingWaitTime <= 0, el ataque sería instantáneo o más rápido de lo que dura la animación.
        }

        // Si el bucle termina, limpiamos la referencia
        attackRoutine = null;
        // El movimiento se reanuda en OnTriggerExit (si es el caso)
    }
}