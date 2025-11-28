using System.Collections;
using UnityEngine;

public class ColliderHealth : MonoBehaviour
{
    private Coroutine attackRoutine;
    
    [Header("Configuración de Ataque")]
    public float attackRate = 1.5f; 
    public float hitDelay = 0.3f; 
    public int damageAmount = 1;


    public PlayerController playerDamage;
    private Enemy enemy;
    

    private bool playerInside = false;

    private void Start()
    {
        playerDamage = FindAnyObjectByType<PlayerController>(); 
        enemy = GetComponentInParent<Enemy>();

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            if (!playerInside)
            {
                playerInside = true;
                
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

            if (attackRoutine != null)
            {
                StopCoroutine(attackRoutine);
                attackRoutine = null;
            }

            enemy?.ResumeMovement();
        }
    }

    private IEnumerator AttackRoutine()
    {
        enemy?.StopMovement(); 
        
        while (playerInside)
        {
            enemy?.PlayAttack();

            yield return new WaitForSeconds(hitDelay); 
            

            if (playerInside && playerDamage != null)
            {
                playerDamage.TakeDamagePlayer(damageAmount);
            }

            float remainingWaitTime = attackRate - hitDelay;
            
            if (remainingWaitTime > 0)
            {
                 yield return new WaitForSeconds(remainingWaitTime);
            }
        }

        attackRoutine = null;
    }
}