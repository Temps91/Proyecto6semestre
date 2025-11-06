using System.Collections;
using UnityEngine;

public class MisteryBoxController : MonoBehaviour
{
    [Header("Armas disponibles")]
    public GameObject[] weaponPrefabs;
    public Transform spawnWeaponPrefab;

    [Header("Times")]
    public float timeRoll = 2f;
    public float timeWeaponPick = 3f;

    public bool IsOpen;
    private GameObject currentWeaponInstance;
    public GameObject colliderWeapon;
    public GameObject colliderBox;

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
        colliderBox.SetActive(false);
        yield return new WaitForSeconds(timeRoll);

        int randomIndex = Random.Range(0, weaponPrefabs.Length);
        GameObject selectedPrefab = weaponPrefabs[randomIndex];

        colliderWeapon.SetActive(true);
        currentWeaponInstance = Instantiate(selectedPrefab, spawnWeaponPrefab.position, spawnWeaponPrefab.rotation, spawnWeaponPrefab);

        yield return new WaitForSeconds(timeWeaponPick);

        if (currentWeaponInstance != null)
        {
            Destroy(currentWeaponInstance);
            currentWeaponInstance = null;
        }

        ResetBoxState();
    }
    public GameObject TakeWeapon()
    {
        if (currentWeaponInstance != null)
        {
            GameObject weaponToGive = currentWeaponInstance;
            currentWeaponInstance = null;
            ResetBoxState();
            return weaponToGive;
        }
        return null;
    }
    private void ResetBoxState()
    {
        IsOpen = false;
        if (colliderBox != null) colliderBox.SetActive(true);
        if (colliderWeapon != null) colliderWeapon.SetActive(false);
    }
}

