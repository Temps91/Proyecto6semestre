using UnityEngine;

public class Zone : MonoBehaviour
{
    public GameObject[] spawners;
    public bool hasPlayer;
    private bool isSpawning = false;
    public void PlayerEntered()
    {
        hasPlayer = true;
        if (!isSpawning)
        {
            isSpawning = true;
            EnemySpawner sp = FindObjectOfType<EnemySpawner>();
            if (sp != null) sp.StartSpawning();
        }
}

    public void PlayerExited()
    {
        hasPlayer = false;
        isSpawning = false;
    }
}