using UnityEngine;

public class cThirdPersonMotor : MonoBehaviour
{
    public float moveSpeed = 5f;
    protected Rigidbody rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public virtual void Move(Vector3 direction)
    {
        Vector3 velocity = direction * moveSpeed;
        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;
    }
}
