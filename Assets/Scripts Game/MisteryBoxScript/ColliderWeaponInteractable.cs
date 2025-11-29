using TMPro;
using UnityEngine;

public class ColliderWeaponInteractable : MonoBehaviour
{
    public TextMeshProUGUI textWeapon;
    public MisteryBoxController controllerBox;
    public Inventory inventory;
    public bool playerInRangeWeapon;
    public Animator weaponBoxAnim;
    bool interact;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRangeWeapon = true;
            textWeapon.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRangeWeapon = false;
            textWeapon.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        interact = SimpleInput.GetButtonDown("Interact");
        if (playerInRangeWeapon && interact)
        {
            textWeapon.gameObject.SetActive(false);
            GameObject weaponPickup = controllerBox.TakeWeapon();
            if (weaponPickup == null) return;
            WeaponBehaviour pickupWB = weaponPickup.GetComponentInChildren<WeaponBehaviour>();
            if (pickupWB == null || pickupWB.weaponData == null || pickupWB.weaponData.weaponPrefab == null)
            {
                ReturnWeaponToBox(weaponPickup);
                return;
            }
            Inventory inv = inventory;
            if (inv == null)
            {
                GameObject player = GameObject.FindWithTag("Player");
                if (player == null)
                {
                    ReturnWeaponToBox(weaponPickup);
                    return;
                }
                inv = player.GetComponent<Inventory>();
                if (inv == null)
                {
                    ReturnWeaponToBox(weaponPickup);
                    return;
                }
            }

            GameObject prefab = pickupWB.weaponData.weaponPrefab;
            int slot = inv.AddOrReplaceWeaponFromPrefab(prefab);

            Destroy(weaponPickup);
            if (controllerBox != null)
                controllerBox.colliderWeapon.SetActive(false);
        }
    }

    private void ReturnWeaponToBox(GameObject weapon)
    {
        if (weapon == null) return;
        if (controllerBox != null && controllerBox.spawnWeaponPrefab != null)
        {
            weapon.transform.SetParent(controllerBox.spawnWeaponPrefab);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
            controllerBox.colliderWeapon.SetActive(true);
        }
        else
        {
            Destroy(weapon);
        }
    }
}
