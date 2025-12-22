using System;
using UnityEngine;
using static Invector.vCharacterController.vThirdPersonMotor;

public class cThirdPersonController : cThirdPersonAnimator
{
    public virtual void ControlAnimatorRootMotion()
    {
        if (!this.enabled) return;
        if (inputSmooth == Vector3.zero)
        {
            transform.position = animator.rootPosition;
            transform.rotation = animator.rootRotation;
        }
        if (useRootMotion)
            MoveCharacter(moveDirection);
    }

    public virtual void ControlLocomotionType()
    {
        if(lockMovement) return;

        if (locomotionType.Equals(LocomotionType.FreeWithStrafe) && !isStrafing || locomotionType.Equals(LocomotionType.OnlyFree))
        {
            SetControllerMoveSpeed(freeSpeed);
            SetAnimatorMoveSpeed(freeSpeed);
        }
        else if (locomotionType.Equals(LocomotionType.OnlyStrafe) || locomotionType.Equals(LocomotionType.FreeWithStrafe) && isStrafing)
        {
            isStrafing = true;
            SetControllerMoveSpeed(strafeSpeed);
            SetAnimatorMoveSpeed(strafeSpeed);
        }

        if (!useRootMotion)
            MoveCharacter(moveDirection);
    }

    protected virtual void UpdateMoveDirection(Transform referenceTransform = null)
    {
        if (input.magnitude <= 0.01)
        {
            moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, (isStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * Time.deltaTime);
        }
        if (referenceTransform && !rotateByWorld)
        {
            //get the right-facing direction of the referenceTransform
            var right = referenceTransform.right;
            right.y = 0;
            //get the forward direction relative to referenceTransform Right
            var forward = Quaternion.AngleAxis(-90, Vector3.up) * right;
            // determine the direction the player will face based on input and the referenceTransform's right and forward directions
            moveDirection = (inputSmooth.x * right) + (inputSmooth.z * forward);
        }
        else
        {
            moveDirection = new Vector3(inputSmooth.x, 0, inputSmooth.z);
        }

    }

}    
