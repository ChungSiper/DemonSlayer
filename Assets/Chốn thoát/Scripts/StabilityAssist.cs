using UnityEngine;

public class StabilityAssist : MonoBehaviour
{
    public float stabilityStrength = 5f;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Stabilize(bool braking)
    {
        if (!braking) return;

        float angularY = rb.angularVelocity.y;
        rb.angularVelocity = new Vector3(
            rb.angularVelocity.x,
            Mathf.Lerp(angularY, 0f, Time.fixedDeltaTime * stabilityStrength),
            rb.angularVelocity.z
        );
    }
}
