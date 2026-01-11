using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class CarController : MonoBehaviour
{
    [Header("Wheel Colliders")]
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;

    [Header("Wheel Meshes")]
    public Transform meshFL;
    public Transform meshFR;
    public Transform meshRL;
    public Transform meshRR;

    [Header("Car Settings")]
    public float motorForce = 1500f;
    public float maxSteerAngle = 30f;
    public float brakeForce = 3000f;

    [Header("Drift")]
    public float driftStiffness = 0.5f;
    public float normalStiffness = 1.5f;

    [Header("Nitro")]
    public float nitroForce = 3000f;
    public float nitroDuration = 3f;

    private float nitroTimer;
    private bool usingNitro;


    [Header("Extra")]
    public float handBrakeForce = 5000f;

    private float horizontal;
    private float vertical;
    private bool isDrifting;
    private bool isBraking;
    private bool isHandBraking;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Hạ trọng tâm xe cho ổn định
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
    }

    void Update()
    {
        // Input
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        isBraking = Input.GetKey(KeyCode.Space);
        isHandBraking = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("isDrifting true!");
            StartDrift();
        }
        if(Input.GetKeyUp(KeyCode.V))
        {
            Debug.Log("isDrifting false!");
            StopDrift();

        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Debug.Log("Nitro Activated!");
            ActiveNitro();
        }

    }

    void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        HandleBrake();
        UpdateWheels();
        HandleNitro();
    }

    void HandleMotor()
    {
        wheelFL.steerAngle = vertical * maxSteerAngle;
        wheelFR.steerAngle = vertical * maxSteerAngle;

        wheelRL.motorTorque = vertical * motorForce;
        wheelRR.motorTorque = vertical * motorForce;

        if(horizontal == 0)
        {
            wheelRL.brakeTorque = brakeForce;
            wheelRR.brakeTorque = brakeForce;
        }
        else
        {
            wheelRL.brakeTorque = 0;
            wheelRR.brakeTorque = 0;
        }
    }
    void StartDrift()
    {
        isDrifting = true;
        SetWheelStiffness(normalStiffness);

    }
    void StopDrift()
    {
        isDrifting = false;
        SetWheelStiffness(driftStiffness);
    }

    void SetWheelStiffness(float value)
    {
        WheelFrictionCurve f = wheelFL.sidewaysFriction;
        f.stiffness = value;

        wheelFL.sidewaysFriction = f;
        wheelFR.sidewaysFriction = f;
    }
    void ActiveNitro()
    {
        if (!usingNitro) return;
        {
            usingNitro = true;
            nitroTimer = nitroDuration;
        }
    }
    void HandleNitro()
    {
        if (!usingNitro) return;
        rb.AddForce(transform.forward * nitroForce);
        nitroTimer -= Time.fixedDeltaTime;

        if (nitroTimer <= 0f)
        {
            usingNitro = false;
        }
    }

    void HandleSteering()
    {
        float steer = maxSteerAngle * horizontal;
        wheelFL.steerAngle = steer;
        wheelFR.steerAngle = steer;
    }

    void HandleBrake()
    {
        float brake = isBraking ? brakeForce : 0f;

        wheelFL.brakeTorque = brake;
        wheelFR.brakeTorque = brake;
        wheelRL.brakeTorque = brake;
        wheelRR.brakeTorque = brake;

        // Phanh tay – khóa bánh sau
        if (isHandBraking)
        {
            wheelRL.brakeTorque = handBrakeForce;
            wheelRR.brakeTorque = handBrakeForce;
        }
    }

    void UpdateWheels()
    {
        UpdateWheelPose(wheelFL, meshFL);
        UpdateWheelPose(wheelFR, meshFR);
        UpdateWheelPose(wheelRL, meshRL);
        UpdateWheelPose(wheelRR, meshRR);
    }

    void UpdateWheelPose(WheelCollider collider, Transform mesh)
    {
        Vector3 pos;
        Quaternion rot;
        collider.GetWorldPose(out pos, out rot);

        mesh.position = pos;
        mesh.rotation = rot;
    }
}
