using UnityEngine;

public class Knife : MonoBehaviour
{
    public int damageKnife;
    private void OnTriggerEnter(Collider other)
    {
        Enemy enemies = other.GetComponent<Enemy>();
        if (enemies != null)
        {
            enemies.TakeDamage(damageKnife);
        }

    }
}
