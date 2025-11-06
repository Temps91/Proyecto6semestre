using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisteryBoxController : MonoBehaviour
{
    [Header("Armas disponibles")]
    public GameObject[] weaponPrefabs;
    public Transform spawnWeaponPrefab;

    [Header("Times")]
    public float timeRoll = 2f;
    public float timeWeaponPick = 3f;
    public float cooldownAfterPickup = 5f;

    public bool IsOpen;
    private GameObject currentWeaponInstance;
    public GameObject colliderWeapon;
    public GameObject colliderBox;

    private bool weaponTakenDuringOpen = false;

    public void Start()
    {
        ResetBoxState();
    }

    public void OpenMisteryBox()
    {
        if (!IsOpen)
            StartCoroutine(OpenBoxRoutine());
    }

    private IEnumerator OpenBoxRoutine()
    {
        IsOpen = true;
        if (colliderBox != null) colliderBox.SetActive(false);
        yield return new WaitForSeconds(timeRoll);

        HashSet<Weapon> excludedWeaponData = new HashSet<Weapon>();

        GameObject playerGO = GameObject.FindWithTag("Player");
        PlayerShooting ps = null;
        Inventory inv = null;

        if (playerGO != null)
        {
            ps = playerGO.GetComponent<PlayerShooting>();
            if (ps == null)
            {
                ps = playerGO.GetComponentInChildren<PlayerShooting>(true);
            }
            if (ps == null)
            {
                ps = FindObjectOfType<PlayerShooting>();
            }

            inv = playerGO.GetComponent<Inventory>();
            if (inv == null)
            {
                inv = playerGO.GetComponentInChildren<Inventory>(true);
            }
            if (inv == null)
            {
                inv = FindObjectOfType<Inventory>();
            }
        }

        if (ps != null && ps.currentWeapon != null && ps.currentWeapon.weaponData != null)
        {
            excludedWeaponData.Add(ps.currentWeapon.weaponData);
        }

        if (inv != null && inv.weaponSlots != null)
        {
            for (int i = 0; i < inv.weaponSlots.Length; i++)
            {
                var slot = inv.weaponSlots[i];
                if (slot == null) continue;
                var wb = slot.GetComponentInChildren<WeaponBehaviour>();
                if (wb != null && wb.weaponData != null)
                {
                    excludedWeaponData.Add(wb.weaponData);
                }
            }
        }

        List<GameObject> candidates = new List<GameObject>();
        for (int i = 0; i < weaponPrefabs.Length; i++)
        {
            var prefab = weaponPrefabs[i];
            if (prefab == null) continue;

            var wb = prefab.GetComponentInChildren<WeaponBehaviour>();
            if (wb == null || wb.weaponData == null)
            {
                candidates.Add(prefab);
            }
            else
            {
                var wd = wb.weaponData;
                bool excluded = excludedWeaponData.Contains(wd);
                if (!excluded)
                    candidates.Add(prefab);
            }
        }

        if (candidates.Count == 0)
        {
            if (colliderWeapon != null) colliderWeapon.SetActive(false);
            yield return new WaitForSeconds(timeWeaponPick);
            ResetBoxState();
            yield break;
        }

        int randomIndex = Random.Range(0, candidates.Count);
        GameObject selectedPrefab = candidates[randomIndex];

        if (colliderWeapon != null) colliderWeapon.SetActive(true);
        currentWeaponInstance = Instantiate(selectedPrefab, spawnWeaponPrefab.position, spawnWeaponPrefab.rotation, spawnWeaponPrefab);

        yield return new WaitForSeconds(timeWeaponPick);

        if (currentWeaponInstance != null)
        {
            Destroy(currentWeaponInstance);
            currentWeaponInstance = null;
            ResetBoxState();
            yield break;
        }

        if (weaponTakenDuringOpen)
        {
            weaponTakenDuringOpen = false;
            yield break;
        }

        ResetBoxState();
    }

    public GameObject TakeWeapon()
    {
        if (currentWeaponInstance != null)
        {
            GameObject weaponToGive = currentWeaponInstance;
            currentWeaponInstance = null;

            weaponTakenDuringOpen = true;

            if (colliderWeapon != null) colliderWeapon.SetActive(false);

            StartCoroutine(CooldownAfterPickupRoutine());

            return weaponToGive;
        }
        return null;
    }

    private IEnumerator CooldownAfterPickupRoutine()
    {
        IsOpen = true;

        if (colliderBox != null) colliderBox.SetActive(false);

        yield return new WaitForSeconds(cooldownAfterPickup);

        if (colliderBox != null) colliderBox.SetActive(true);
        IsOpen = false;
    }

    private void ResetBoxState()
    {
        IsOpen = false;
        if (colliderBox != null) colliderBox.SetActive(true);
        if (colliderWeapon != null) colliderWeapon.SetActive(false);
    }
}