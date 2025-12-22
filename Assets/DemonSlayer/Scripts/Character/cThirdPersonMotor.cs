using System;
using UnityEngine;
using static Invector.vCharacterController.vThirdPersonMotor;

public class cThirdPersonMotor : MonoBehaviour
{
    [SerializeField]
    public class cMovementSpeed
    {
        [Range(1f, 20f)]
        [Tooltip("Độ mượt khi thay đổi hướng và tốc độ di chuyển của nhân vật")]
        public float movementSmooth = 6f;

        [Range(0f, 1f)]
        [Tooltip("Độ mượt khi chuyển đổi animation (blend animation)")]
        public float animationSmooth = 0.2f;

        [Tooltip("Tốc độ xoay của nhân vật")]
        public float rotationSpeed = 16f;

        [Tooltip("Nhân vật mặc định chỉ đi bộ, không chạy")]
        public bool walkByDefault = false;

        [Tooltip("Nhân vật sẽ xoay theo hướng camera khi đứng yên")]
        public bool rotateWithCamera = false;

        [Tooltip("Tốc độ đi bộ khi dùng Rigidbody hoặc tốc độ cộng thêm nếu dùng Root Motion")]
        public float walkSpeed = 2f;

        [Tooltip("Tốc độ chạy khi dùng Rigidbody hoặc tốc độ cộng thêm nếu dùng Root Motion")]
        public float runningSpeed = 4f;

        [Tooltip("Tốc độ chạy nước rút khi dùng Rigidbody hoặc tốc độ cộng thêm nếu dùng Root Motion")]
        public float sprintSpeed = 6f;
    }

    public bool useRootMotion = false;

    public LocomotionType locomotionType = LocomotionType.FreeWithStrafe;
    public cMovementSpeed freeSpeed, strafeSpeed;


    public bool rotateByWorld = false;
    public LayerMask groundLayer;
    public float groundMinDistance = 0.2f;
    public float groundMaxDistance = 0.5f;
    #region Components
    internal Animator animator;
    internal Rigidbody rigidbody;
    internal PhysicsMaterial FrictionPhysics, maxFrictionPhysics, slippyPhysics;
    internal CapsuleCollider _capsuleCollider;
    #endregion
    #region Internal  Variables
    internal bool isJumping;
    internal bool isStrafing
    {
        get
        {
            return _isStrafing;
        }
        set
        {
            _isStrafing = value;
        }
    }
    internal bool isGrounded { get; set; }
    internal bool isSprinting { get; set; }
    public bool stopMove { get; protected set; }

    internal float inputMagnitude;
    internal float horizontalSpeed;
    internal float verticalSpeed;
    internal float moveSpeed;
    internal float colliderHeight, colliderRadius;
    internal bool _isStrafing;
    internal float groundDistance;
    internal bool lockMovement = false;
    internal bool lockRotation = false;
    internal Transform rotateTarget;
    internal Vector3 input;
    internal  Vector3 moveDirection;
    internal Vector3 inputSmooth;
    internal Vector3 colliderCenter;
    #endregion
    public void Init()
    {
        animator = GetComponent<Animator>();
        animator.updateMode = AnimatorUpdateMode.Fixed;

        rigidbody = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        colliderHeight = _capsuleCollider.height;
        colliderRadius = _capsuleCollider.radius;
        colliderCenter = _capsuleCollider.center;
        _capsuleCollider = GetComponent<CapsuleCollider>();
         isGrounded = true;
    }
    protected virtual void UpdateMotor()
    {
        CheckGround();
        CheckSlopeLimit();
        ControlJumpBehaviour();
        AirControl();
    }
    public virtual void SetControllerMoveSpeed(cMovementSpeed speed)
    {
        if (speed.walkByDefault)
            moveSpeed = Mathf.Lerp(moveSpeed, isSprinting ? speed.runningSpeed : speed.walkSpeed, speed.movementSmooth * Time.deltaTime);
        else
            moveSpeed = Mathf.Lerp(moveSpeed, isSprinting ? speed.sprintSpeed : speed.runningSpeed, speed.movementSmooth * Time.deltaTime);
    }
    protected virtual void CheckGround()
    {
        CheckGroundDistace();
        ControlMaterialPhysics();

    }
    public virtual void MoveCharacter(Vector3 moveDirection)
    {
        inputSmooth = Vector3.Lerp(inputSmooth, input, (isStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * Time.deltaTime);
        if (!isGrounded || isJumping) return;
        moveDirection.y = 0;
        moveDirection.x = Mathf.Clamp(moveDirection.x, -1f, 1f);
        moveDirection.z = Mathf.Clamp(moveDirection.z, -1f, 1f);
        if (moveDirection.magnitude > 1f)
            moveDirection.Normalize();

        Vector3 targetPosition = useRootMotion ? animator.rootPosition : rigidbody.position + moveDirection * (stopMove ? 0 : moveSpeed) * Time.deltaTime;
        Vector3 targetVelocity = (targetPosition - transform.position) / Time.deltaTime;

        bool useVerticalVelocity = true;
        if (useVerticalVelocity) targetVelocity.y = rigidbody.linearVelocity.y;
        rigidbody.linearVelocity = targetVelocity;

    }
    protected virtual void ControlMaterialPhysics()
    {
        throw new NotImplementedException();
    }

    protected virtual void CheckGroundDistace()
    {
        if(_capsuleCollider != null)
        {
            float radius = _capsuleCollider.radius * 2f;
            var dist = 15f;

            Ray ray2 = new Ray(transform.position + new Vector3(0, colliderHeight / 2, 0), Vector3.down);
            if(Physics.Raycast(ray2, out RaycastHit groundHit, (colliderHeight / 2) + dist) && !groundHit.collider.isTrigger)
                dist = transform.position.y - groundHit.point.y;
            if(dist >= groundMinDistance)
            {
                Vector3 pos = transform.position + Vector3.up *(_capsuleCollider.radius);
                Ray ray = new Ray(pos, -Vector3.up);
                if (Physics.SphereCast(ray, radius, out groundHit, _capsuleCollider.radius + groundMaxDistance, groundLayer) && !groundHit.collider.isTrigger)
                {
                    Physics.Linecast(groundHit.point + (Vector3.up * 0.1f), groundHit.point + Vector3.down * 0.15f, out groundHit, groundLayer);
                    float newDist = transform.position.y - groundHit.point.y;
                    if (dist > newDist) dist = newDist;
                }
            }
            groundDistance = (float)System.Math.Round(dist, 2);
        }
    }

    protected virtual void CheckSlopeLimit()
    {
        throw new NotImplementedException();
    }


    protected virtual void ControlJumpBehaviour()
    {
        throw new NotImplementedException();
    }

    protected virtual void AirControl()
    {
        throw new NotImplementedException();
    }

}
