using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public GameObject[] powerUpPrefabs;
    public int[] powerUpProbabilities;
    public float baseDropChance = 5f;


    public void AttemptDrop(Vector3 dropPos)
    {
        float randomGlobal = Random.Range(0, powerUpProbabilities.Length);
        int randomPower = Random.Range(0, powerUpPrefabs.Length);

        if (randomGlobal > baseDropChance)
        {
            return;
        }

        int totalWeight = 0;

        foreach (int weight in powerUpProbabilities)
        {
            totalWeight += weight;
        }

        int randomTarget = Random.Range(0, totalWeight);
        int currentWeight = 0;

        for(int i = 0; i < powerUpProbabilities.Length; i++)
        {
            currentWeight += powerUpProbabilities[i];

            if (randomTarget < currentWeight)
            {
                Instantiate(powerUpPrefabs[randomPower], dropPos, Quaternion.identity);
                return;
            }
        }
    }
}
