using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour
{
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
    }

    private void Shoot()
    {
        Vector3 startPos = transform.position;
        Vector3 direction = transform.forward;
        Vector3 endPos = startPos + direction * currentWeapon.weaponData.range;

        Debug.DrawRay(startPos, direction * currentWeapon.weaponData.range, Color.red, 0.2f);

        if (Physics.Raycast(startPos, direction, out RaycastHit hit, currentWeapon.weaponData.range))
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(currentWeapon.weaponData.damage);
            }
            endPos = hit.point;
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
            Destroy(currentWeaponModel);

        currentWeapon = newWeapon;

        currentWeapon.ammo = currentWeapon.weaponData.ammo;
        currentWeapon.currentMagazine = currentWeapon.weaponData.magazineSize;

        if (newWeapon.weaponData.weaponPrefab != null)
        {
            currentWeaponModel = Instantiate(newWeapon.weaponData.weaponPrefab, playerHand);
            currentWeaponModel.transform.localPosition = Vector3.zero;
            currentWeaponModel.transform.localRotation = Quaternion.identity;
        }

    }
}

