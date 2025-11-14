using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Componentes")]
    public GameManager gameManager;
    public NavMeshAgent agent;
    public Transform player;
    public GameObject attackCollider;
    public bool playerStay;

    [Header("Stats de Vida")]
    public int healthMax = 10;
    public int healthCurrent;
    public float speedEnemy = 3.5f;

    private void Awake()
    {
        healthCurrent = healthMax;

        if (player == null)
        {
            GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO != null)
                player = playerGO.transform;
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
        if (player == null || agent == null) return;

        agent.SetDestination(player.position);
    }

    public void TakeDamage(int amount)
    {
        healthCurrent -= amount;
        gameManager.PointsAgree(10);
        if (healthCurrent <= 0)
            Die();
    }


    private void Die()
    {
        gameManager.PointsAgree(100);
        gameManager.EnemyKilled();
        gameObject.SetActive(false);
    }
}

