using System.Collections;
using UnityEngine;

public class ColliderHealth : MonoBehaviour
{
    private Coroutine hitRoutine;
    public float hitTimer;
    public float attackDuration = 1.5f;
    public PlayerController playerDamage;
    private Enemy enemy;
    private bool isAttackingPlayer = false;
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
            playerInside = true;

            if (!isAttackingPlayer)
            {
                isAttackingPlayer = true;
                enemy?.StopMovement();
                enemy?.PlayAttack();

                if (hitRoutine == null)
                    hitRoutine = StartCoroutine(AttackRoutine());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            playerInside = false;
            isAttackingPlayer = false;

            if (hitRoutine != null)
            {
                StopCoroutine(hitRoutine);
                hitRoutine = null;
            }

            enemy?.ResumeMovement();
        }
    }

    private IEnumerator AttackRoutine()
    {

        while (playerInside)
        {
            enemy?.StopMovement();
            enemy?.PlayAttack();

            yield return new WaitForSeconds(attackDuration);

            playerDamage?.TakeDamagePlayer(1);

            if (hitTimer > 0f)
                yield return new WaitForSeconds(hitTimer);
        }

        hitRoutine = null;
    }
}
