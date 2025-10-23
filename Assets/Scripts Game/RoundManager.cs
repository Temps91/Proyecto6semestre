using UnityEngine;
using System.Collections;

public class RoundManager : MonoBehaviour
{
    public ZombiePoolManager poolManager;
    public int zombiesPerRound = 24;
    public float spawnInterval = 0.5f;

    void Start()
    {
        StartCoroutine(StartRound());
    }

    void Update()
    {
        if (poolManager.IsRoundOver())
        {
            Debug.Log("Ronda terminada!");
            StartCoroutine(StartRound());
        }
    }

    IEnumerator StartRound()
    {
        for (int i = 0; i < zombiesPerRound; i++)
        {
            poolManager.GetZombie();
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}


