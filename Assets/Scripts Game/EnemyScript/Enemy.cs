using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int healthMax;
    public int healthCurrent;


    public void TakeDamage(int cantidad)
    {
        healthCurrent -= cantidad;

        if (healthCurrent <= 0)
        {
            Dead();
        }
    }
    
    public void Dead()
    {
        gameObject.SetActive(false);
    }
}
