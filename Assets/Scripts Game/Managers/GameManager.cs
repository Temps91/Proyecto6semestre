using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const float DOUBLE_POWER_DURATION = 20f;

    [Header("Double Points PowerUp")]
    public float timerDoublePower = DOUBLE_POWER_DURATION;
    public int iTimerDoublePower;
    public TextMeshProUGUI textDoublePoints;
    public Image doublePointsUI;
    public bool doublePointsBool;

    private Coroutine doublePointsTimerRef;

    public static GameManager Instance;
    public int iTimer;
    public float timer;
    public TextMeshProUGUI text;
    public TextMeshProUGUI textRound;
    public TextMeshProUGUI textPoints;

    [Header("Rondas")]
    public int baseEnemies = 10;
    public int enemiesIncrement = 2;
    public int maxEnemies = 24;

    [Header("Estado actual")]
    public int currentEnemies;
    public int enemiesSpawned;
    public int totalEnemiesThisRound;
    public int roundNumber = 1;

    [Header("Points")]
    public int points;
    public int totalPointsG;


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
        points = 10000;
        if (doublePointsUI != null)
        {
            doublePointsUI.gameObject.SetActive(false);
        }
    }

    public void Update()
    {
        if (doublePointsBool)
        {
            timerDoublePower -= Time.deltaTime;
            iTimerDoublePower = Mathf.CeilToInt(timerDoublePower);
            textDoublePoints.text = iTimerDoublePower.ToString();
            if (timerDoublePower <= 0f)
            {
                DisableDoublePoints();
            }
        }

        if (Input.GetKey(KeyCode.K))
        {
            StartNewRound();
        }

        timer += Time.deltaTime;
        iTimer = (int)timer;
        text.text = iTimer.ToString();
        textRound.text = roundNumber.ToString();
        TotalPoints();
        textPoints.text = points.ToString();
    }

    public void ActivateDoublePoints()
    {
        if (doublePointsTimerRef != null)
        {
            StopCoroutine(doublePointsTimerRef);
        }

        timerDoublePower = DOUBLE_POWER_DURATION;
        doublePointsBool = true;
        doublePointsUI.gameObject.SetActive(true);

        doublePointsTimerRef = StartCoroutine(DoublePointsTimerCoroutine());
    }

    public IEnumerator DoublePointsTimerCoroutine()
    {
        yield return new WaitForSeconds(DOUBLE_POWER_DURATION);

        DisableDoublePoints();
    }

    private void DisableDoublePoints()
    {
        doublePointsBool = false;
        if (doublePointsUI != null)
        {
            doublePointsUI.gameObject.SetActive(false);
        }
        timerDoublePower = DOUBLE_POWER_DURATION;
        doublePointsTimerRef = null;
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

        var spawners = FindObjectsOfType<EnemySpawner>();
        foreach (var s in spawners)
        {
            s.StartSpawning();
        }
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

    public void PointsAgree(int P)
    {
        if (doublePointsBool)
        {
            points += P * 2;
        }
        else
        {
            points += P;
        }

        TotalPoints();
    }

    public void TotalPoints()
    {
        totalPointsG = points;
    }
}
