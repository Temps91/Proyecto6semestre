using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTimer : MonoBehaviour
{
    private const float INSTA_KILL_DURATION = 20f;

    [Header("Componentes UI")]
    public Image instaKillImage;
    public TextMeshProUGUI textInsta;

    [Header("Estado y Timer")]
    public float timerInstaPower = INSTA_KILL_DURATION;
    public int iTimerInstaPower;
    public bool instaKill;

    private Coroutine instaKillTimerRef;

    public void Update()
    {
        if (instaKill)
        {
            timerInstaPower -= Time.deltaTime;
            iTimerInstaPower = Mathf.CeilToInt(timerInstaPower);
            textInsta.text = iTimerInstaPower.ToString();

            if (timerInstaPower <= 0f)
            {
                DisableInstaKill();
            }
        }
    }


    public void ActivateInstaKill()
    {
        if (instaKillTimerRef != null)
        {
            StopCoroutine(instaKillTimerRef);
        }


        timerInstaPower = INSTA_KILL_DURATION;
        instaKill = true;
        instaKillImage.gameObject.SetActive(true);

        instaKillTimerRef = StartCoroutine(InstaKillActiveCoroutine());
        Debug.Log("Insta Kill REINICIADO y activado");
    }

    private IEnumerator InstaKillActiveCoroutine()
    {
        yield return new WaitForSeconds(INSTA_KILL_DURATION);

        DisableInstaKill();
    }

    private void DisableInstaKill()
    {
        instaKill = false;
        if (instaKillImage != null)
        {
            instaKillImage.gameObject.SetActive(false);
        }
        timerInstaPower = INSTA_KILL_DURATION;
        instaKillTimerRef = null;
        Debug.Log("Insta Kill DESACTIVADO");
    }
}