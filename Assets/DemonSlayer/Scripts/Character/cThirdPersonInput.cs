using System;
using UnityEngine;

public class cThirdPersionInput : MonoBehaviour
{
    #region Variables
    [Header("Controller Input")]
    public string horizontalInput = "Horizontal";
    public string verticalInput = "Vertical";
    public KeyCode JumpInput = KeyCode.Space;
    public KeyCode strafeInput = KeyCode.Tab;
    public KeyCode sptintInput = KeyCode.LeftShift;
    
    [Header("Camera Input")]
    public string rotateCameraXInput = "Mouse X";
    public string rotateCameraYInput = "Mouse Y";
    [HideInInspector] public cThirdPersonController cc;
    [HideInInspector] public cThirdPersonCamera tpCamera;
    [HideInInspector] public Camera cameraMain;
    #endregion
    
    protected virtual void Start()
    {
        InitializeController(); 
        InitializeTpCamera();
    }
    protected virtual void FixedUpdate()
    {
        // cc.UpdateMotor();               // updates the ThirdPersonMotor methods
        // cc.ControlLocomotionType();     // handle the controller locomotion type and movespeed
        // cc.ControlLocomotionType(); 
    }
    protected virtual void Update()
    {
        InputHandle();                  // update the input methods
        // cc.UpdateAnimator();            // updates the Animator Parameters
    }
    #region Basic Locomotion Inputs
    protected virtual void InitializeController()
    {
        cc = GetComponent<cThirdPersonController>();
    }

    protected virtual void InitializeTpCamera()
    {
        tpCamera = GetComponent<cThirdPersonCamera>();
        cameraMain = Camera.main;
    }

    protected virtual void InputHandle()
    {
        MoveInput();   
        CameraInput();
        SprintInput();
        StrafeInput();
        HandleJumpInput();
    }
    protected virtual void MoveInput()
    {
        cc.input.x = Input.GetAxis(horizontalInput);
        cc.input.z = Input.GetAxis(verticalInput);
    }

    protected virtual void CameraInput()
    {
        
    }

    protected virtual void SprintInput()
    {
        //if(Input.GetKeyDown(sptintInput))
        //{
        //    cc.Sprint(true);
        //}
        //else if(Input.GetKeyUp(sptintInput))
        //{
        //    cc.Sprint(false);
        //}
    }
    protected virtual void StrafeInput()
    {
        //if(Input.GetKeyDown(strafeInput))
        //{
        //    cc.Strafe();
        //}
    }
    protected virtual void JumpCondition()
    {
        //return cc.isGrounder && cc.GroundAnle() < cc.slopeLimit && !cc.isJumping && !cc.stopMove;
    }
    protected virtual void HandleJumpInput()
    {
        //if(Input.GetKeyDown(JumpInput) && JumpCondition())
        //{
        //    cc.Jump();
        //}
    }
    #endregion
}