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
            Debug.Log($"🔫 {weaponData.name} inicializada con {currentMagazine}/{ammo} balas.");
        }
        else
        {
            Debug.LogWarning($"⚠️ {name} no tiene ScriptableObject asignado.");
        }
    }
}



