using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, Fireable
{

    public float damage = 5f;
    public float range = 5f;
    public float fireRate = 1f;
    private float curFireRate = 0f;
    private bool canShoot = true;
    private float curPower;

    public GameObject Firepoint;

    [SerializeField] private GameObject projectile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool ableToShoot()
    {
        return canShoot;
    }

    public Fireable Shoot(float power)
    {
        if (canShoot == true)
        {
            curPower = power;
            var clone = Instantiate(projectile);
            clone.transform.position = GetComponentInParent<Camera>().transform.position;
            clone.transform.rotation = GetComponentInParent<Camera>().transform.rotation;
            canShoot = false;
            curFireRate = 0f;
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (curFireRate >= fireRate)
        {
            canShoot = true;
        }
        else
        {
            curFireRate += Time.deltaTime;
        }
    }
}
