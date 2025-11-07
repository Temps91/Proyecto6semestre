using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    public Weapon weaponData;
    public int currentMagazine;
    public int ammo;

    private void Awake()
    {
        if (weaponData != null)
        {
            currentMagazine = weaponData.magazineSize;
            ammo = weaponData.ammo;
        }
    }
}



