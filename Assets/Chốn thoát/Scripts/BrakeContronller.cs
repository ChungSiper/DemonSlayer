using UnityEngine;

public class BrakeController : MonoBehaviour
{
    [Header("Brake Settings")]
    public float maxBrakeForce = 4000f;
    [Range(0.5f, 0.9f)]
    public float frontBias = 0.7f;

    public float absSlipLimit = 0.4f;

    public WheelCollider fl, fr, rl, rr;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ApplyBrake(bool braking)
    {
        if (!braking)
        {
            ReleaseBrake();
            return;
        }

        float speedFactor = Mathf.Clamp01(rb.linearVelocity.magnitude / 30f);
        float brake = maxBrakeForce * speedFactor;

        // ABS giả lập
        brake = ApplyABS(brake);

        fl.brakeTorque = brake * frontBias;
        fr.brakeTorque = brake * frontBias;
        rl.brakeTorque = brake * (1f - frontBias);
        rr.brakeTorque = brake * (1f - frontBias);
    }

    void ReleaseBrake()
    {
        fl.brakeTorque = fr.brakeTorque = 0f;
        rl.brakeTorque = rr.brakeTorque = 0f;
    }

    float ApplyABS(float brake)
    {
        WheelHit hit;
        if (fl.GetGroundHit(out hit))
        {
            if (Mathf.Abs(hit.forwardSlip) > absSlipLimit)
                brake *= 0.6f;
        }
        return brake;
    }
}
