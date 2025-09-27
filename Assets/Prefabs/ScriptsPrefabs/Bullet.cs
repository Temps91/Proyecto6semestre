using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody rb;


    public void Start()
    {
        rb.GetComponent<Rigidbody>();
    }

    public void ForceBullet()
    {
        rb.AddForce(Vector3.forward, ForceMode.Force);
    }

}
