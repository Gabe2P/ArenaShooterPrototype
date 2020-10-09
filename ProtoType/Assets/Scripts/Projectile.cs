using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour, Throwable, Interactable
{
    Transform parent = null;
    Rigidbody rb = null;
    Camera cam = null;

    private bool isheld = false;

    LayerMask ogLayer;
    private float curTime = 0f;
    public float timer = 1f;

    private float lastPower = 0f;
    // Start is called before the first frame update
    void Start()
    {
        cam = PlayerManager.instance.player.GetComponentInChildren<Camera>();
        parent = gameObject.transform.parent;
        rb = gameObject.GetComponent<Rigidbody>();
        ogLayer = gameObject.layer;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.parent == parent)
        {
            if (gameObject.layer != ogLayer)
            {
                if (curTime < timer)
                {
                    curTime += Time.fixedDeltaTime;
                }
                else
                {
                    curTime = 0;
                    gameObject.layer = ogLayer;
                }
            }
        }
        else
        {
            curTime = 0;
        }

    }

    public Interactable Activate()
    {
        rb.isKinematic = true;
        gameObject.transform.SetParent(cam.transform);
        gameObject.layer = cam.gameObject.layer;
        isheld = true;
        return null;
    }

    public Interactable Deactivate()
    {
        rb.isKinematic = false;
        gameObject.transform.SetParent(parent);
        isheld = false;
        return null;
    }

    public Throwable Throw(float power)
    {
        Deactivate();
        lastPower = power;
        rb.AddForce(cam.transform.forward * power, ForceMode.Impulse);
        return null;
    }
}
