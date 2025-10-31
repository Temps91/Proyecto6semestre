using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int enemiesIncrement;
    public int enemyCount;
    public int currentEnemies;


    private void Awake()
    {
        // Si ya existe una instancia y no somos nosotros, destruir este objeto
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Asignar la instancia
        Instance = this;

        // Hacer que el objeto no se destruya al cambiar de escena
        DontDestroyOnLoad(gameObject);
    }
    public void EnemyKilled()
    {
        enemyCount--;
        if(enemyCount == 0)
        {
            ChangeRound();
        }
    }

    public void ChangeRound()
    {
        enemyCount += enemiesIncrement;
        currentEnemies = enemyCount;
        
    }
}
