using Invector.vCharacterController;
using UnityEngine;

public class cThirdPersonController : cThirdPersonAnimator
{
    private cThirdPersonInput input;
    private cThirdPersonMotor motor;
    private Transform cam;

    protected override void Start()
    {
        base.Start();

        input = GetComponent<cThirdPersonInput>();
        motor = GetComponent<cThirdPersonMotor>();
        cam = Camera.main.transform;
    }

    void FixedUpdate()
    {
        HandleMovement();
        UpdateAnimator(input.horizontal, input.vertical, input.magnitude);
    }

    void HandleMovement()
    {
        if (input.magnitude <= 0.01f)
        {
            motor.Move(Vector3.zero);
            return;
        }

        Vector3 camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveDir = camForward * input.vertical + cam.right * input.horizontal;

        motor.Move(moveDir.normalized);
    }
}
