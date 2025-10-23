using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Weapon currentWeapon;
    private float nextTimeToFire = 0f;

    private void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / currentWeapon.fireRate;
            Shoot();
        }
    }
    public void Shoot()
    {
        Ray ray =  new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, currentWeapon.range))
        {

        }
    }
}