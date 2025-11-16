using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class PlayerShooting : MonoBehaviour
{
    public Image hitImage;
    public TextMeshProUGUI ammoCurrentText;
    public TextMeshProUGUI magazineCurrentText;
    [Header("CurrentWeapon")]
    public WeaponBehaviour currentWeapon;
    public Transform playerHand;
    private GameObject currentWeaponModel;

    [Header("Shoot")]
    private float nextTimeToFire = 0f;
    public LineRenderer lineRenderer;
    public float lineDuration = 0.05f;

    private void Update()
    {
        if (currentWeapon == null)
            return;

        Reload();
        ReloadAuto();

        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
        {
            if (currentWeapon.currentMagazine > 0)
            {
                nextTimeToFire = Time.time + 1f / currentWeapon.weaponData.fireRate;
                Shoot();
                currentWeapon.currentMagazine--;
            }
            else
            {
            }
        }

        ammoCurrentText.text = currentWeapon.ammo.ToString();
        magazineCurrentText.text = currentWeapon.currentMagazine.ToString();
    }

    private void Shoot()
    {
        Vector3 startPos = transform.position;
        Vector3 direction = transform.forward;
        Vector3 endPos = startPos + direction * currentWeapon.weaponData.range;

        Debug.DrawRay(startPos, direction * currentWeapon.weaponData.range, Color.red, 0.2f);

        RaycastHit[] hits = Physics.RaycastAll(startPos, direction, currentWeapon.weaponData.range);
        if (hits.Length > 0)
        {
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            float maxHitDistance = 0f;
            foreach (var hit in hits)
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(currentWeapon.weaponData.damage);
                    StartCoroutine(hitCorrutine());
                }

                if (hit.distance > maxHitDistance)
                    maxHitDistance = hit.distance;
            }

            endPos = startPos + direction * maxHitDistance;
        }

        if (lineRenderer != null)
            StartCoroutine(DrawRay(startPos, endPos));
    }

    private IEnumerator DrawRay(Vector3 start, Vector3 end)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        yield return new WaitForSeconds(lineDuration);
        lineRenderer.enabled = false;
    }

    private void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            int neededAmmo = currentWeapon.weaponData.magazineSize - currentWeapon.currentMagazine;
            if (neededAmmo <= 0 || currentWeapon.ammo <= 0) return;

            int ammoToLoad = Mathf.Min(neededAmmo, currentWeapon.ammo);
            currentWeapon.currentMagazine += ammoToLoad;
            currentWeapon.ammo -= ammoToLoad;

        }
    }

    private void ReloadAuto()
    {
        if (currentWeapon.currentMagazine > 0 || currentWeapon.ammo <= 0) return;

        int neededAmmo = currentWeapon.weaponData.magazineSize;
        int ammoToLoad = Mathf.Min(neededAmmo, currentWeapon.ammo);
        currentWeapon.currentMagazine = ammoToLoad;
        currentWeapon.ammo -= ammoToLoad;

    }

    public void SetWeapon(WeaponBehaviour weapon)
    {
        EquipWeapon(weapon);
    }

    private void EquipWeapon(WeaponBehaviour newWeapon)
    {
        if (currentWeaponModel != null)
        {
            GameObject possibleModel = currentWeaponModel;
            if (possibleModel != null)
                possibleModel.SetActive(false);

            currentWeaponModel = null;
        }

        currentWeapon = newWeapon;

        if (currentWeapon != null)
        {
            currentWeapon.ammo = currentWeapon.weaponData.ammo;
            currentWeapon.currentMagazine = currentWeapon.weaponData.magazineSize;
        }

        GameObject modelFromScene = null;
        if (newWeapon != null && playerHand != null && newWeapon.transform.IsChildOf(playerHand))
        {
            Transform t = newWeapon.transform;
            while (t.parent != null && t.parent != playerHand)
                t = t.parent;
            if (t.parent == playerHand)
                modelFromScene = t.gameObject;
        }

        if (modelFromScene != null)
        {
            currentWeaponModel = modelFromScene;
            currentWeaponModel.transform.localPosition = Vector3.zero;
            currentWeaponModel.transform.localRotation = Quaternion.identity;
            currentWeaponModel.SetActive(true);
            return;
        }

        if (newWeapon != null && newWeapon.weaponData != null && newWeapon.weaponData.weaponPrefab != null)
        {
            currentWeaponModel = Instantiate(newWeapon.weaponData.weaponPrefab, playerHand);
            currentWeaponModel.transform.localPosition = Vector3.zero;
            currentWeaponModel.transform.localRotation = Quaternion.identity;

        }
    }

    IEnumerator hitCorrutine()
    {
        hitImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        hitImage.gameObject.SetActive(false);
    }
}

