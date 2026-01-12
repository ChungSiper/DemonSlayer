using UnityEngine;

public class DriftController : MonoBehaviour
{
    public float driftStiffness = 0.5f;
    public float normalStiffness = 1.5f;
    public float minDriftSpeed = 15f;

    public float driftNitroRate = 15f; // tích nitro / giây

    public WheelCollider rearLeft;
    public WheelCollider rearRight;

    public bool IsDrifting { get; private set; }

    Rigidbody rb;
    NitroController nitro;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        nitro = GetComponent<NitroController>();
    }

    public void HandleDrift(float steerInput)
    {
        float speed = rb.linearVelocity.magnitude * 3.6f;

        if (Input.GetKey(KeyCode.Space) && speed > minDriftSpeed && Mathf.Abs(steerInput) > 0.1f)
        {
            if (!IsDrifting) StartDrift();
            nitro.AddNitro(driftNitroRate * Time.deltaTime);
        }
        else
        {
            if (IsDrifting) StopDrift();
        }
    }

    void StartDrift()
    {
        IsDrifting = true;
        SetStiffness(driftStiffness);
    }

    void StopDrift()
    {
        IsDrifting = false;
        SetStiffness(normalStiffness);
    }

    void SetStiffness(float value)
    {
        WheelFrictionCurve f = rearLeft.sidewaysFriction;
        f.stiffness = value;
        rearLeft.sidewaysFriction = f;
        rearRight.sidewaysFriction = f;
    }
}

