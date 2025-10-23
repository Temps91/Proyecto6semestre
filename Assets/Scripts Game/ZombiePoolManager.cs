using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZombiePoolManager : MonoBehaviour
{
    public GameObject zombiePrefab;
    public int zombiesPerRound = 24;
    public Transform player;
    public Transform[] spawnPoints;

    private List<GameObject> zombies;

    void Awake()
    {
        zombies = new List<GameObject>();

        for (int i = 0; i < zombiesPerRound; i++)
        {
            GameObject zombie = Instantiate(zombiePrefab);
            zombie.SetActive(false);
            zombies.Add(zombie);
        }
    }

    public GameObject GetZombie()
    {
        foreach (GameObject zombie in zombies)
        {
            if (!zombie.activeInHierarchy)
            {
                Transform[] sortedSpawns = spawnPoints.OrderBy(sp => Vector3.Distance(player.position, sp.position)).ToArray();
                int maxIndex = Mathf.Min(5, sortedSpawns.Length);
                Transform chosenSpawn = sortedSpawns[Random.Range(0, maxIndex)];
                zombie.transform.position = chosenSpawn.position;
                zombie.transform.rotation = chosenSpawn.rotation;
                zombie.SetActive(true);
                return zombie;
            }
        }

        return null;
    }


    public void ReturnZombie(GameObject zombie)
    {
        zombie.SetActive(false);
    }

    public void StartRound()
    {
        int count = 0;
        foreach (GameObject zombie in zombies)
        {
            if (!zombie.activeInHierarchy && count < zombiesPerRound)
            {
                zombie.SetActive(true);
                count++;
            }
        }
    }
    public bool IsRoundOver()
    {
        foreach (GameObject zombie in zombies)
        {
            if (zombie.activeInHierarchy)
                return false;
        }
        return true;
    }
}
