using UnityEngine;

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
            AddWeapon(startWeapon);
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

    public int AddWeapon(GameObject weaponInstance)
    {
        int slotToUse = (weaponSlots[0] == null) ? 0 : (weaponSlots[1] == null ? 1 : -1);

        if (slotToUse == -1)
        {
            return -1;
        }

        weaponSlots[slotToUse] = weaponInstance;

        if (slotToUse == currentSlot)
        {
            var weaponBehaviour = weaponInstance.GetComponentInChildren<WeaponBehaviour>();
            playerShooting.SetWeapon(weaponBehaviour);
            weaponInstance.SetActive(true);
        }
        else
        {
            weaponInstance.SetActive(false);
        }
        return slotToUse;
    }


    private void EquipWeapon(GameObject newWeaponPrefab, int slotIndex)
    {
        if (weaponSlots[slotIndex] != null)
            Destroy(weaponSlots[slotIndex]);

        GameObject weaponInstance = Instantiate(newWeaponPrefab, playerHand);
        weaponInstance.transform.localPosition = Vector3.zero;
        weaponInstance.transform.localRotation = Quaternion.identity;

        weaponSlots[slotIndex] = weaponInstance;

        var weaponBehaviour = weaponInstance.GetComponentInChildren<WeaponBehaviour>();

        if (slotIndex == currentSlot)
        {
            playerShooting.SetWeapon(weaponBehaviour);
            weaponInstance.SetActive(true);
        }
        else
        {
            weaponInstance.SetActive(false);
        }

    }

    private void ChangeToSlot(int slotIndex)
    {
        if (slotIndex == currentSlot) return;
        if (weaponSlots[slotIndex] == null)
        {
            return;
        }

        weaponSlots[slotIndex].SetActive(true);
        if (weaponSlots[currentSlot] != null)
            weaponSlots[currentSlot].SetActive(false);

        currentSlot = slotIndex;

        var weaponBehaviour = weaponSlots[slotIndex].GetComponentInChildren<WeaponBehaviour>();
        playerShooting.SetWeapon(weaponBehaviour);

    }
}

