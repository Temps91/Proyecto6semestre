using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;
    public float damage;
    public float range;
    public float fireRate;
    public int ammo;
    public bool infiniteAmmo = false;
}
