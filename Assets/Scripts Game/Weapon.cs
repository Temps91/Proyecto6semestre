using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;
    public int damage;
    public float range;
    public float fireRate;
    public int ammo;
    public int magazineSize;
    public bool infiniteAmmo = false;
    public int currentMagazine;
    public GameObject weaponPrefab;

    public bool isAutomatic = false;

    public float reloadTime = 1.5f;

    private void OnEnable()
    {
        currentMagazine = magazineSize;
    }
}
