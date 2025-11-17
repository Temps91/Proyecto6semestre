using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefab y zonas")]
    public GameObject prefabEnemy;
    public Zone[] zone;
    [Header("Opciones")]
    public float spawnTime = 2f;

    [Header("Timing")]
    public float initialSpawnDelay = 3f;
    public float spawnInterval = 1f;

    [Header("Dificultad")]
    public int healthIncreasePerRound = 2;

    private bool isSpawning = false;

    public void StartSpawning()
    {
        if (!isSpawning)
        {
            StartCoroutine(SpawnZombies());
        }
    }

    private IEnumerator SpawnZombies()
    {
        isSpawning = true;

        yield return new WaitForSeconds(initialSpawnDelay);

        while (GameManager.Instance != null && GameManager.Instance.CanSpawnMore())
        {
            Zone playerZone = null;
            foreach (Zone z in zone)
            {
                if (z == null) continue;
                if (z.hasPlayer)
                {
                    playerZone = z;
                    break;
                }
            }

            if (playerZone != null)
            {
                SpawnZombie(playerZone);
                GameManager.Instance.RegisterSpawn();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
        isSpawning = false;
    }

    private void SpawnZombie(Zone spawnZone)
    {
        if (spawnZone == null)
        {
            return;
        }

        if (spawnZone.spawners == null || spawnZone.spawners.Length == 0)
        {
            return;
        }

        int i = Random.Range(0, spawnZone.spawners.Length);
        GameObject sp = spawnZone.spawners[i];

        if (sp == null)
        {
            return;
        }

        if (prefabEnemy == null)
        {
            return;
        }

        GameObject go = Instantiate(prefabEnemy, sp.transform.position, sp.transform.rotation);
        Enemy enemy = go.GetComponent<Enemy>();
        if (enemy != null)
        {
            int round = GameManager.Instance != null ? GameManager.Instance.roundNumber : 1;
            int prefabBaseHealth = enemy.healthMax;
            int newHealth = Mathf.Max(1, prefabBaseHealth + (round - 1) * healthIncreasePerRound);
            enemy.healthMax = newHealth;
            enemy.healthCurrent = enemy.healthMax;
        }
    }
}