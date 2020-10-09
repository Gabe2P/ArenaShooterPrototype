using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Written by Gabriel Tupy 9/3/2019 @ 6:56pm

[RequireComponent(typeof(PlayerMotor))]

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 2f;
    public float sprintSpeed = 4f;
    public float playerJumpSpeed = 5.5f;

    public float maxPower = 2f;
    public float chargeRate = 1f;
    private float curPower = 0f;


    public bool isCrouched;

    [SerializeField]
    private float lookSens = 2f;

    [SerializeField]
    private float grav = 25f;

    private PlayerMotor motor;

    // Start is called before the first frame update
    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        Physics.gravity = new Vector3(0f, -grav, 0f);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //creating temp value for player speed calculations
        float _playerSpeed = playerSpeed;

        //Calculating Input
        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMov = Input.GetAxisRaw("Vertical");


        //Reseting Values
        Vector3 _jump = Vector3.zero;

        //Calculating Movement Vectors
        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;

        //Calculating Player Input for picking up objects
        if (Input.GetButtonDown("Use"))
        {
            motor.Activate();
        }

        //Calculating "Shooting"
        if (Input.GetButton("Fire1"))
        {
            if (curPower < maxPower)
            {
                curPower += Time.deltaTime * chargeRate;
            }
            else
            {
                curPower = maxPower;
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            motor.Shoot(curPower);
            curPower = 0f;
        }

        //Calculating Throw
        if (Input.GetButton("Fire2"))
        {
            if (curPower < maxPower)
            {
                curPower += Time.deltaTime;
            }
            else
            {
                curPower = maxPower;
            }
        }
        if (Input.GetButtonUp("Fire2"))
        {
            motor.Throw(curPower);
            curPower = 0f;
        }

        //Calculating Crouching Input
        if (Input.GetButtonDown("Crouch"))
        {
            motor.Crouch();
            isCrouched = true;
        }
        if (Input.GetButtonUp("Crouch"))
        {
            motor.unCrouch();
            isCrouched = false;
        }

        //Calculating Sprinting Input
        if (Input.GetButton("Sprint") && !isCrouched)
        {
            _playerSpeed = sprintSpeed;
        }

        //Calculating Jumping Input
        if (Input.GetButtonDown("Jump") && !isCrouched)
        {
            _jump = transform.up * playerJumpSpeed;
        }

        //Applying jumping vector
        motor.Jump(_jump);


        //Final movement Vector
        Vector3 _velocity = (_movHorizontal + _movVertical).normalized * _playerSpeed;

        //Applying movement vector
        motor.Move(_velocity);


        //Calculating mouse Movement for only the y axis
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSens;

        //applying mouse movement
        motor.Rotate(_rotation);


        //Calculating mouse Movement for only the x axis
        float _xRot = Input.GetAxisRaw("Mouse Y");

        float _cameraRotationX = _xRot * lookSens;


        //applying mouse movement
        motor.RotateCamera(_cameraRotationX);
    }
}
