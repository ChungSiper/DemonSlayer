using UnityEngine;

public class cThirdPersonAnimator : cThirdPersonMotor
{
    public const float walkSpeed = 0.5f;
    public const float runningSpeed = 1f;
    public const float sprintSpeed = 1.5f;
    public virtual void UpdateAnimator()
    {
        if (animator == null || !animator.enabled) return;
        animator.SetBool(cAnimatorParameters.IsStrafing, isStrafing);
        animator.SetBool(cAnimatorParameters.IsSprinting, isSprinting);
        animator.SetBool(cAnimatorParameters.IsGrounded, isGrounded);
        animator.SetFloat(cAnimatorParameters.GroundDistance, groundDistance);
        if (isStrafing)
        {
            animator.SetFloat(cAnimatorParameters.InputHorizontal, stopMove ? 0 : horizontalSpeed, strafeSpeed.animationSmooth, Time.deltaTime);
            animator.SetFloat(cAnimatorParameters.InputVertical, stopMove ? 0 : verticalSpeed, strafeSpeed.animationSmooth, Time.deltaTime);
        }
        else
        {
            animator.SetFloat(cAnimatorParameters.InputVertical, stopMove ? 0 : verticalSpeed, freeSpeed.animationSmooth, Time.deltaTime);
        }
        animator.SetFloat(cAnimatorParameters.InputMagnitude, stopMove ? 0f : inputMagnitude, isStrafing ? strafeSpeed.animationSmooth : freeSpeed.animationSmooth, Time.deltaTime);
    }
    public virtual void SetAnimatorMoveSpeed(cMovementSpeed speed)
    {
        Vector3 relativeInput = transform.InverseTransformDirection(moveDirection);
        verticalSpeed = relativeInput.z;
        horizontalSpeed = relativeInput.x;
        var newInput = new Vector2(verticalSpeed, horizontalSpeed);
        if (speed.walkByDefault)
            inputMagnitude = Mathf.Clamp(newInput.magnitude, 0, isSprinting ? runningSpeed : walkSpeed);
        else
            inputMagnitude = Mathf.Clamp(isSprinting ? newInput.magnitude + 0.5f : newInput.magnitude, 0, isSprinting ? sprintSpeed : runningSpeed);
    }
    public static partial class cAnimatorParameters
    {
        public static int InputHorizontal = Animator.StringToHash("InputHorizontal");
        public static int InputVertical = Animator.StringToHash("InputVertical");
        public static int InputMagnitude = Animator.StringToHash("InputMagnitude");
        public static int IsGrounded = Animator.StringToHash("IsGrounded");
        public static int IsStrafing = Animator.StringToHash("IsStrafing");
        public static int IsSprinting = Animator.StringToHash("IsSprinting");
        public static int GroundDistance = Animator.StringToHash("GroundDistance");
    }
}
