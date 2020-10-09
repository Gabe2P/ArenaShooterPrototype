using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField]private float lifeSpan;
    private float power;
    private Transform firePoint;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * PlayerManager.instance.player.GetComponent<PlayerMotor>().GetPower();
        Invoke("Destroy", lifeSpan);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform != PlayerManager.instance.player.transform)
        {
            Invoke("Destroy", 1f);
        }
    }

    private void Destroy()
    {
        Destroy(this.gameObject);
    }
}
