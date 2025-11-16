using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefab y zonas")]
    public GameObject prefabEnemy;
    public Zone[] zone;
    [Header("Opciones")]
    public float spawnTime = 2f;

    [Header("Dificultad")]
    // Incremento porcentual por cada ronda extra (0.2 = +20% por ronda)
    public float healthMultiplierPerRound = 0.2f;

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

            yield return new WaitForSeconds(spawnTime);
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

        // Instanciamos y ajustamos la vida según la ronda
        GameObject go = Instantiate(prefabEnemy, sp.transform.position, sp.transform.rotation);
        Enemy enemy = go.GetComponent<Enemy>();
        if (enemy != null)
        {
            int round = GameManager.Instance != null ? GameManager.Instance.roundNumber : 1;
            float scale = 1f + healthMultiplierPerRound * (round - 1);
            enemy.healthMax = Mathf.Max(1, Mathf.RoundToInt(enemy.healthMax * scale));
            enemy.healthCurrent = enemy.healthMax;
        }
    }
}