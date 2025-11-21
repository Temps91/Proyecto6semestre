using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class PlayerShooting : MonoBehaviour
{
    // Esta variable será activada por el script MaxAmmoPowerUp.cs
    public bool maxAmmo;
    public Image maxAmmoImage;
    [Header("Text")]
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

    private bool isReloading = false;

    [Header("Reload Rotation")]
    public bool rotateWeaponOnReload = true;
    public Vector3 reloadRotationAxis = Vector3.up;
    public float reloadRotationDegrees = 360f;
    public bool resetRotationAfterReload = true;

    [Header("Reload Movement")]
    public bool moveWeaponOnReload = true;
    public Vector3 reloadMoveOffset = new Vector3(0f, -0.1f, -0.25f);
    public bool resetPositionAfterReload = true;

    [Range(0f, 1f)]
    public float easingPower = 0.5f;

    private void Update()
    {
        if (currentWeapon == null)
            return;

        if (maxAmmo)
        {
            maxAmmoImage.gameObject.SetActive(true);
            ApplyMaxAmmo();
            StartCoroutine(MaxAmmoAc());
            
        }
        // ----------------------------------------------------

        if (!isReloading)
        {
            if (Input.GetKeyDown(KeyCode.R))
                TryStartReload();

            if (currentWeapon.currentMagazine <= 0 && currentWeapon.ammo > 0)
                TryStartReload();
        }

        bool wantsToFire = currentWeapon.weaponData != null && currentWeapon.weaponData.isAutomatic
            ? Input.GetMouseButton(0)
            : Input.GetMouseButtonDown(0);

        if (!isReloading && wantsToFire && Time.time >= nextTimeToFire)
        {
            if (currentWeapon.currentMagazine > 0)
            {
                nextTimeToFire = Time.time + 1f / currentWeapon.weaponData.fireRate;
                Shoot();
                currentWeapon.currentMagazine--;
            }
            else
            {
                // Manejar sonido de click vacío aquí si es necesario
            }
        }

        ammoCurrentText.text = currentWeapon.ammo.ToString();
        magazineCurrentText.text = isReloading ? "Recargando..." : currentWeapon.currentMagazine.ToString();
    }

    public void ApplyMaxAmmo()
    {
        // Obtener la referencia al inventario (asumiendo que está en el mismo GameObject)
        Inventory inventory = GetComponent<Inventory>();

        if (inventory == null)
        {
            Debug.LogError("Error: PlayerShooting requiere el script Inventory en el mismo GameObject.");
            maxAmmo = false; // Desactivar para evitar un bucle de error
            return;
        }

        // Iteramos sobre todos los slots de armas del inventario
        for (int i = 0; i < inventory.weaponSlots.Length; i++)
        {
            GameObject weaponSlot = inventory.weaponSlots[i];

            if (weaponSlot != null)
            {
                // Obtener el WeaponBehaviour de la instancia del arma
                WeaponBehaviour weaponBehaviour = weaponSlot.GetComponentInChildren<WeaponBehaviour>();

                if (weaponBehaviour != null && weaponBehaviour.weaponData != null)
                {
                    // Recargar la munición de reserva (ammo) al valor máximo del ScriptableObject
                    weaponBehaviour.ammo = weaponBehaviour.weaponData.ammo;

                    // Recargar el cargador actual (currentMagazine) al máximo
                    weaponBehaviour.currentMagazine = weaponBehaviour.weaponData.magazineSize;
                }
            }
        }

        maxAmmo = false;
        Debug.Log("Max Ammo Recargado en todas las armas!");
    }
    // ----------------------------------------------------

    private void TryStartReload()
    {
        if (currentWeapon == null || currentWeapon.weaponData == null) return;
        if (isReloading) return;

        int neededAmmo = currentWeapon.weaponData.magazineSize - currentWeapon.currentMagazine;
        if (neededAmmo <= 0) return;
        if (currentWeapon.ammo <= 0 && !currentWeapon.weaponData.infiniteAmmo) return;

        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        isReloading = true;

        float reloadDuration = currentWeapon.weaponData.reloadTime;

        if ((rotateWeaponOnReload || moveWeaponOnReload) && currentWeaponModel != null)
        {
            Vector3 originalPos = currentWeaponModel.transform.localPosition;
            Quaternion originalRot = currentWeaponModel.transform.localRotation;
            Vector3 axis = reloadRotationAxis.normalized;
            float elapsed = 0f;

            while (elapsed < reloadDuration)
            {
                float dt = Time.deltaTime;
                elapsed += dt;
                float t = Mathf.Clamp01(elapsed / reloadDuration);
                float easedT = easingPower <= 0f ? t : Mathf.SmoothStep(0f, 1f, t);

                if (moveWeaponOnReload)
                {
                    Vector3 targetPos = originalPos + reloadMoveOffset;
                    currentWeaponModel.transform.localPosition = Vector3.Lerp(originalPos, targetPos, easedT);
                }

                if (rotateWeaponOnReload)
                {
                    float currentAngle = Mathf.Lerp(0f, reloadRotationDegrees, easedT);
                    currentWeaponModel.transform.localRotation = originalRot * Quaternion.AngleAxis(currentAngle, axis);
                }

                yield return null;
            }

            if (moveWeaponOnReload)
            {
                Vector3 finalPos = originalPos + reloadMoveOffset;
                if (resetPositionAfterReload)
                    currentWeaponModel.transform.localPosition = originalPos;
                else
                    currentWeaponModel.transform.localPosition = finalPos;
            }

            if (rotateWeaponOnReload)
            {
                Quaternion finalRot = originalRot * Quaternion.AngleAxis(reloadRotationDegrees, axis);
                if (resetRotationAfterReload)
                    currentWeaponModel.transform.localRotation = originalRot;
                else
                    currentWeaponModel.transform.localRotation = finalRot;
            }
        }
        else
        {
            yield return new WaitForSeconds(reloadDuration);
        }

        int neededAmmo = currentWeapon.weaponData.magazineSize - currentWeapon.currentMagazine;
        if (neededAmmo > 0)
        {
            if (currentWeapon.weaponData.infiniteAmmo)
            {
                currentWeapon.currentMagazine = currentWeapon.weaponData.magazineSize;
            }
            else
            {
                int ammoToLoad = Mathf.Min(neededAmmo, currentWeapon.ammo);
                currentWeapon.currentMagazine += ammoToLoad;
                currentWeapon.ammo -= ammoToLoad;
            }
        }

        isReloading = false;
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
                // Asume que tienes un script Enemy
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

    IEnumerator MaxAmmoAc()
    {
        yield return new WaitForSeconds(2f);
        maxAmmoImage.gameObject.SetActive(false);
    }
}