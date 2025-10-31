using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject prefabEnemy;
    public Zone[] zone;
    public GameObject player;
    public float spawnTime;


    public void SpawnZombie(Zone spawnZone)
    {

        int i = Random.Range(0, spawnZone.spawners.Length);
        Instantiate(prefabEnemy, spawnZone.spawners[i].transform);
    }

    public IEnumerator SpawnZombies()
    {
        for(int i = 0; i < GameManager.Instance.enemyCount; i++)
        {

            for (int j = 0; j < zone.Length; j++)
            {

                if (zone[j].hasPlayer)
                {
                    SpawnZombie(zone[j]);
                }

            }
            yield return new WaitForSeconds(spawnTime);
        }
    }


    


}
