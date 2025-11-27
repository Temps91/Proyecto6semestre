using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    
    public bool wasInstaKillActiv;
    [Header("Componentes")]
    public EnemyTimer instaTimer;
    public PowerUpManager powerUp;
    public GameManager gameManager;
    public NavMeshAgent agent;
    public Transform player;
    public GameObject attackCollider;
    public bool playerStay;

    [Header("Animacion")]
    public Animator animator;
    public string attackTrigger = "Attack";
    public string jumpTrigger = "Jump";
    public string runBool = "Run";
    public string deathTrigger = "Die";


    [Header("Stats de Vida")]
    public int healthMax = 10;
    public int healthCurrent;
    public float speedEnemy = 3.5f;
    public float slowSpeedWindows;

    private void Awake()
    {
        healthCurrent = healthMax;
        if (instaTimer == null)
        {
            instaTimer = FindAnyObjectByType<EnemyTimer>();
        }
        if (player == null)
        {
            GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO != null)
                player = playerGO.transform;
        }
        if (powerUp == null)
        {
            powerUp = FindAnyObjectByType<PowerUpManager>();
        }

        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.speed = speedEnemy;
        }

        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (instaTimer.instaKill)
        {
            healthCurrent = 1;
            wasInstaKillActiv = true;
        }
        else if (wasInstaKillActiv)
        {
            healthCurrent = healthMax;
            wasInstaKillActiv = false;
        }

        if (player == null || agent == null) return;

        agent.SetDestination(player.position);

        if (agent.isOnOffMeshLink)
        {
            agent.speed = slowSpeedWindows;
            animator.SetTrigger(jumpTrigger);
        }
        else
        {
            agent.speed = speedEnemy;
        }

    }

    public void TakeDamage(int amount)
    {
        healthCurrent -= amount;

        if (healthCurrent <= 0)
            Die();
        else 
        {
            if(gameManager != null)
                gameManager.PointsAgree(10);
        }
    }


    private void Die()
    {

        if (gameManager != null)
            gameManager.PointsAgree(100);

        gameManager.EnemyKilled();
        animator.SetTrigger(deathTrigger);
        powerUp.AttemptDrop(transform.position);
        gameObject.SetActive(false);
    }

    public void StopMovement()
    {
        if (agent != null)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
    }

    public void ResumeMovement()
    {
        if (agent != null)
        {
            agent.isStopped = false;
        }
    }
    public void PlayAttack()
    {
        if (animator != null && !string.IsNullOrEmpty(attackTrigger))
        {
            animator.SetTrigger(attackTrigger);
        }
    }
}

