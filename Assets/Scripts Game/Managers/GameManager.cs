using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Rondas")]
    public int baseEnemies = 10;
    public int enemiesIncrement = 2;
    public int maxEnemies = 24;

    [Header("Estado actual")]
    public int currentEnemies;
    public int enemiesSpawned;
    public int totalEnemiesThisRound;
    public int roundNumber = 1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartNewRound();
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.K))
        {
            StartNewRound();
        }
    }

    public void EnemyKilled()
    {
        currentEnemies--;
        if (currentEnemies <= 0 && enemiesSpawned >= totalEnemiesThisRound)
        {
            StartNewRound();
        }
    }

    public void StartNewRound()
    {
        if (roundNumber > 1)
        {
            baseEnemies += enemiesIncrement;
        }

        if (baseEnemies > maxEnemies)
        {
            baseEnemies = maxEnemies;
        }

        totalEnemiesThisRound = baseEnemies;
        enemiesSpawned = 0;
        currentEnemies = 0;
        roundNumber++;
    }

    public bool CanSpawnMore()
    {
        return enemiesSpawned < totalEnemiesThisRound;
    }

    public void RegisterSpawn()
    {
        enemiesSpawned++;
        currentEnemies++;
    }
}
