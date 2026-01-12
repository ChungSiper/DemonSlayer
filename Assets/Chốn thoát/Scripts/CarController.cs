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
    public float minDriftSpeed = 15f;

    [Header("Nitro")]
    public float nitroForce = 3000f;
    public float nitroDuration = 3f;

    [Header("Hand Brake")]
    public float MaxBarkeForce = 5000f;
    public float frontBias = 0.7f;
    public float absSlipLimit = 0.4f;

    private float horizontal;
    private float vertical;
    private bool isBraking;
    private bool isDrifting;
    private bool usingNitro;
    private float nitroTimer;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        float speed = rb.linearVelocity.magnitude * 3.6f;
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            isBraking = true;
            HandleBrake(isBraking);
        }
        // Drift (GIỮ SPACE)
        if (Input.GetKey(KeyCode.Space) && speed > minDriftSpeed && Mathf.Abs(horizontal) > 0.1f)
        {
            if (!isDrifting) StartDrift();
        }
        else
        {
            if (isDrifting) StopDrift();
        }

        // Nitro
        if (Input.GetKeyDown(KeyCode.LeftControl))
            ActivateNitro();
    }

    void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        
        HandleNitro();
        UpdateWheels();
    }

    void HandleMotor()
    {
        wheelRL.motorTorque = vertical * motorForce;
        wheelRR.motorTorque = vertical * motorForce;
    }

    void HandleSteering()
    {
        float steer = horizontal * maxSteerAngle;
        wheelFL.steerAngle = steer;
        wheelFR.steerAngle = steer;
    }

    void HandleBrake(bool isBraking)
    {
        if (!isBraking)
        {
            wheelFL.brakeTorque = wheelFR.brakeTorque = 0f;
            wheelRL.brakeTorque = wheelRR.brakeTorque = 0f;
            return;
        }
        float speedFactor = Mathf.Clamp01(rb.linearVelocity.magnitude / 30f);
        float brake = brakeForce * speedFactor;
        WheelHit hit;
        if (wheelFL.GetGroundHit(out hit))
        {
            if (Mathf.Abs(hit.forwardSlip) > absSlipLimit)
                brake *= 0.6f;
        }

        wheelFL.brakeTorque = brake * frontBias;
        wheelFR.brakeTorque = brake * frontBias;
        wheelRL.brakeTorque = brake * (1f - frontBias);
        wheelRR.brakeTorque = brake * (1f - frontBias);
    }

    // ===== DRIFT =====
    void StartDrift()
    {
        isDrifting = true;
        SetRearWheelStiffness(driftStiffness);
    }

    void StopDrift()
    {
        isDrifting = false;
        SetRearWheelStiffness(normalStiffness);
    }

    void SetRearWheelStiffness(float value)
    {
        WheelFrictionCurve f = wheelRL.sidewaysFriction;
        f.stiffness = value;

        wheelRL.sidewaysFriction = f;
        wheelRR.sidewaysFriction = f;
    }

    // ===== NITRO =====
    void ActivateNitro()
    {
        if (usingNitro) return;

        usingNitro = true;
        nitroTimer = nitroDuration;
    }

    void HandleNitro()
    {
        if (!usingNitro) return;

        rb.AddForce(transform.forward * nitroForce, ForceMode.Acceleration);
        nitroTimer -= Time.fixedDeltaTime;

        if (nitroTimer <= 0f)
            usingNitro = false;
    }

    // ===== WHEELS =====
    void UpdateWheels()
    {
        UpdateWheelPose(wheelFL, meshFL);
        UpdateWheelPose(wheelFR, meshFR);
        UpdateWheelPose(wheelRL, meshRL);
        UpdateWheelPose(wheelRR, meshRR);
    }

    void UpdateWheelPose(WheelCollider col, Transform mesh)
    {
        Vector3 pos;
        Quaternion rot;
        col.GetWorldPose(out pos, out rot);
        mesh.position = pos;
        mesh.rotation = rot;
    }
}
