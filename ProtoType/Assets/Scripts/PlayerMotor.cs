using UnityEngine;

//Written by Gabriel Tupy 9/3/2019 @ 6:56pm

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(CapsuleCollider))]

public class PlayerMotor : MonoBehaviour
{
    //Camera Variables
    [SerializeField] private Camera cam;
    private float cameraRotationX = 0f;
    private float currentCameraRotationX = 0f;
    [SerializeField]private float cameraRotationLimit = 80f;


    //Crouching Variables
    private bool isCrouched = false;
    public float CrouchOffset = 1f;


    //Power and Recoil Variables
    [SerializeField]private Transform recoilPoint;
    private float lastPower = 0f;
    public float powerMultiplier = 10f;
    public float dampeningRecoilPower = 2f;
    private Vector3 recoilDir = Vector3.zero;


    //Movement and Rotation Variables
    private Vector3 jumpVelocity = Vector3.zero;
    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    [SerializeField] private float maxAirTime = 2f;
    private float curAirTime;


    //Collision Detection and Interaction Detection Variables
    [SerializeField]private float interactionRange = 5f;
    [SerializeField]private LayerMask GroundLayer;
    [SerializeField]private LayerMask Interactable;
    public SphereCollider GroundCheck;
    public CapsuleCollider Hitbox;
    private Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public float GetPower()
    {
        return lastPower;
    }

    public void Throw(float curPower)
    {
        lastPower = curPower * powerMultiplier;
        recoilDir = (recoilPoint.position - cam.transform.position).normalized * (lastPower / dampeningRecoilPower);

        Throwable heldItem = cam.GetComponentInChildren<Throwable>();
        if (heldItem != null)
        {
            rb.AddForce(recoilDir, ForceMode.Impulse);
            heldItem.Throw(lastPower);
        }
    }

    public void Shoot(float curPower)
    {
        lastPower = curPower * powerMultiplier;
        recoilDir = (recoilPoint.position - cam.transform.position).normalized * (lastPower / dampeningRecoilPower);
        Weapon heldItem = cam.GetComponentInChildren<Weapon>();
        if (heldItem != null && heldItem.ableToShoot())
        {
            rb.AddForce(recoilDir, ForceMode.Impulse);
            heldItem.Shoot(lastPower);
        }

    }

    public void Activate()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, interactionRange, Interactable))
        {
            Interactable heldItem = cam.transform.GetComponentInChildren<Interactable>();
            Interactable Obj = hitInfo.transform.GetComponent<Interactable>();

            Debug.Log(hitInfo.transform.name);

            if (heldItem == null)
            {
                if (Obj != null)
                {
                    Obj.Activate();
                }
            }
            else
            {
                heldItem.Deactivate();
            }
        }
    }

    //Gets a Crouch input
    public void Crouch()
    {
        Hitbox.enabled = false;
        cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y - CrouchOffset, cam.transform.position.z);
    }

    public void unCrouch()
    {
        Hitbox.enabled = true;
        cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y + CrouchOffset, cam.transform.position.z);
    }


    //Gets a jump movement vector
    public void Jump(Vector3 _jumpVelocity)
    {
        jumpVelocity = _jumpVelocity;
    }

    //Gets a mouse movement Vector
    public void RotateCamera(float _cameraRotationX)
    {
        cameraRotationX = _cameraRotationX;
    }

    //Gets a mouse movement Vector
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    // Gets a movement Vector
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PerformMovement();
        PerformJump();
        PerformRotation();
    }


    //Ground Collision Check
    private bool isGrounded()
    {
        return Physics.CheckCapsule(GroundCheck.bounds.center, new Vector3(GroundCheck.bounds.center.x, GroundCheck.bounds.min.y, GroundCheck.bounds.center.z), GroundCheck.radius * .9f, GroundLayer);
    }

    //Perform Jump
    void PerformJump()
    {
        if (jumpVelocity != Vector3.zero && isGrounded())
        {
            rb.AddForce(jumpVelocity, ForceMode.Impulse);
        }
    }

    //perform movement
    void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.deltaTime);
        }
    }

    //perform rotation/mouse movement
    void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

        if (cam != null)
        {
            //Set Rotation and Clamp Rotation
            currentCameraRotationX -= cameraRotationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);


            //Applying the Camera Rotation
            cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        }
    }

}
