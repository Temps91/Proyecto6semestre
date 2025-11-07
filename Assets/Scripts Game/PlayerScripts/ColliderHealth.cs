using System.Collections;
using UnityEngine;

public class ColliderHealth : MonoBehaviour
{
    private IEnumerator HitCoroutine;
    public float hitTimer;
    public PlayerController playerDamage;

    private void Start()
    {
        playerDamage = FindAnyObjectByType<PlayerController>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            if (HitCoroutine == null)
            {
                HitCoroutine = WaitForHit();
                StartCoroutine(HitCoroutine);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent <PlayerController>() != null)
        {
            if (HitCoroutine != null)
            {
                StopCoroutine(HitCoroutine);
                HitCoroutine = null;
            }
        }
    }

    public IEnumerator WaitForHit()
    {
        yield return new WaitForSeconds(hitTimer);
        HitCoroutine = null;
        playerDamage.TakeDamagePlayer(1);
    }
}
