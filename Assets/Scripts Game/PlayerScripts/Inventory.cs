using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour
{
    [Header("WeaponsSlot")]
    public GameObject[] weaponSlots = new GameObject[2];
    public PlayerShooting playerShooting;
    public Transform playerHand;

    public GameObject startWeapon;

    private int currentSlot = 0;

    private void Start()
    {
        if (startWeapon != null)
            AddWeaponFromPrefab(startWeapon);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeToSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeToSlot(1);
        }
    }

    public int CurrentSlot => currentSlot;
    public int AddWeapon(GameObject weaponInstance)
    {
        // Buscar primer slot libre
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] == null)
            {
                weaponSlots[i] = weaponInstance;

                if (i == currentSlot)
                {
                    var weaponBehaviour = weaponInstance.GetComponentInChildren<WeaponBehaviour>();
                    playerShooting.SetWeapon(weaponBehaviour);
                    weaponInstance.SetActive(true);
                }
                else
                {
                    weaponInstance.SetActive(false);
                }

                return i;
            }
        }

        return -1;
    }

    public int AddWeaponFromPrefab(GameObject newWeaponPrefab)
    {
        if (newWeaponPrefab == null) return -1;

        // Buscar primer slot libre
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] == null)
            {
                GameObject weaponInstance = Instantiate(newWeaponPrefab, playerHand);
                weaponInstance.transform.localPosition = Vector3.zero;
                weaponInstance.transform.localRotation = Quaternion.identity;

                weaponSlots[i] = weaponInstance;

                var weaponBehaviour = weaponInstance.GetComponentInChildren<WeaponBehaviour>();

                if (i == currentSlot)
                {
                    playerShooting.SetWeapon(weaponBehaviour);
                    weaponInstance.SetActive(true);
                }
                else
                {
                    weaponInstance.SetActive(false);
                }

                return i;
            }
        }

        return -1;
    }

    public int AddOrReplaceWeaponFromPrefab(GameObject newWeaponPrefab)
    {
        if (newWeaponPrefab == null) return -1;

        int slot = AddWeaponFromPrefab(newWeaponPrefab);
        if (slot != -1)
            return slot;

        ReplaceWeaponInSlot(currentSlot, newWeaponPrefab);
        return currentSlot;
    }

    public void ReplaceWeaponInSlot(int slotIndex, GameObject newWeaponPrefab)
    {
        if (slotIndex < 0 || slotIndex >= weaponSlots.Length) return;

        if (weaponSlots[slotIndex] != null)
            Destroy(weaponSlots[slotIndex]);

        GameObject weaponInstance = Instantiate(newWeaponPrefab, playerHand);
        weaponInstance.transform.localPosition = Vector3.zero;
        weaponInstance.transform.localRotation = Quaternion.identity;

        weaponSlots[slotIndex] = weaponInstance;

        var weaponBehaviour = weaponInstance.GetComponentInChildren<WeaponBehaviour>();
        if (slotIndex == currentSlot)
            playerShooting.SetWeapon(weaponBehaviour);

        weaponInstance.SetActive(slotIndex == currentSlot);
    }

    private void ChangeToSlot(int slotIndex)
    {
        if (slotIndex == currentSlot) return;
        if (slotIndex < 0 || slotIndex >= weaponSlots.Length) return;
        if (weaponSlots[slotIndex] == null) return;

        if (weaponSlots[currentSlot] != null)
            weaponSlots[currentSlot].SetActive(false);

        weaponSlots[slotIndex].SetActive(true);

        currentSlot = slotIndex;

        var weaponBehaviour = weaponSlots[slotIndex].GetComponentInChildren<WeaponBehaviour>();
        playerShooting.SetWeapon(weaponBehaviour);
    }
}
