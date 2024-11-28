using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;
public class PlayerMovement : MonoBehaviour
{
    public Vector2 move;
    [HideInInspector] public bool canMove = true;
    private Vector3 moveDir;
    private float turnSmoothVel;
    public readonly float expectedFramerate = 60f;
    private Sliding slideController;
    [SerializeField] private PlayerAnimations playerAnimation;

    [Header("Setup Fields")]
    [SerializeField] private Transform PlayerCamera;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform PlayerModel;
    public float turnSmoothTime = 0.1f;
    public float extraGravity = 1f;
    [Space]
    [Header("Lateral Movement")]
    private float speed;
    public float maxForce, walkSpeed, airSpeed, maxAirSpeedMod, climbSpeed, wallrunSpeed, groundDrag, dashPower, dashCooldownTime;
    private float dashCooldownTimer;
    private PlayerSFXManager playerSounds;

    [Header("Jumping")]
    [SerializeField] private float jumpPower = 500f;
    [SerializeField] private float jumpSpeed = 1f;
    [SerializeField] private float jumpCooldown = .15f;
    private bool readyToJump;
    [HideInInspector] public float jumpInput;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle = 50f;
    private RaycastHit slopeHit;

    [Header("Stat Tracking")]
    public Speedometer velocityDisplay;
    public DashDisplay dashHUD;

    [Header("References")]
    public Climbing climbingScript;

    [HideInInspector] public MovementState state;
    public enum MovementState
    {
        walking,
        air,
        sliding,
        climbing,
        wallrunning,
        notMoving
    }

    [HideInInspector] public bool sliding;
    [HideInInspector] public bool climbing;
    [HideInInspector] public bool wallrunning;

    private void StateHandler()
    {
        // wallrunning
        if(wallrunning)
        {
            state = MovementState.wallrunning;
            speed = wallrunSpeed;
            playerAnimation.PlayWallrunAnimation();
        }
        // climbing
        else if (climbing)
        {
            state = MovementState.climbing;
            speed = climbSpeed;
            playerAnimation.PlayClimbAnimation();
        }

        // sliding
        else if(sliding)
        {
            state = MovementState.sliding;
            rb.linearDamping = 0;
            playerAnimation.PlaySlideAnimation();
        }
        else if (grounded && move.magnitude <= 0.1f)
        {
            state = MovementState.notMoving;
            speed = 0f; 
            rb.linearDamping = groundDrag; 
            playerAnimation.PlayIdleAnimation(); 
        }

        // walking
        else if (grounded)
        {
            state = MovementState.walking;
            speed = walkSpeed;
            rb.linearDamping = groundDrag;
            playerAnimation.PlayRunAnimation();
        }

        // air
        else
        {
            state = MovementState.air;
            speed = airSpeed;
            rb.linearDamping = 0;
            
        }
    }

    private void HandleLoopingSFX()
    {
        // Sliding sfx
        if (!sliding || (sliding && !grounded))
            playerSounds.StopSlideSfx();
        else if (sliding && grounded)
            playerSounds.PlaySlideSfx(rb.linearVelocity.magnitude);

        // Wind SFX
        playerSounds.TryWindSfx(rb.linearVelocity.magnitude);
    }

    private void Update()
    {
        StateHandler();

        HandleLoopingSFX();

        // Ground check
        grounded = Physics.Raycast(rb.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        //  Dash timer
        if (dashCooldownTimer < dashCooldownTime)
        {
            dashCooldownTimer += Time.deltaTime;
            dashCooldownTimer = Mathf.Clamp(dashCooldownTimer, 0f, dashCooldownTime);
        }

        // when to jump
        if (jumpInput > 0 && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
            playerAnimation.PlayAirAnimation();
        }

        // UI
        UpdateUI();
    }

    void FixedUpdate()
    {
        // Movement
        if(canMove)
            MovePlayer();

        // Extra Gravity
        if(!grounded && rb.useGravity)
        {
            rb.AddForce(Vector3.down * extraGravity * (Time.deltaTime * expectedFramerate), ForceMode.Force);
        }
    }

    private void Start()
    {
        playerSounds = GetComponent<PlayerSFXManager>();
        Cursor.lockState = CursorLockMode.Locked;
        slideController = GetComponent<Sliding>();

        if (dashHUD != null)
            dashHUD.SetMaxCooldown(dashCooldownTime);

        readyToJump = true;
    }

    private void UpdateUI()
    {
        Vector3 horizVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        if(velocityDisplay != null)
            velocityDisplay.SetSpeed(horizVel.magnitude);

        if(dashHUD != null)
            dashHUD.SetCooldown(dashCooldownTimer);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
     
        move = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jumpInput = context.ReadValue<float>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        Dash();
    }

    private void MovePlayer()
    {
        if (climbingScript.exitingWall) return;

        // Find Target Velocity
        Vector3 currentVel = rb.linearVelocity;
        Vector3 targetDir = new Vector3(move.x, 0, move.y);

        // Only if trying to move
        if (targetDir.magnitude > 0.1f)
        {
            // Face direction based on camera
            float targetAngle = Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg + PlayerCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(rb.rotation.eulerAngles.y, targetAngle, ref turnSmoothVel, turnSmoothTime);
            rb.rotation = Quaternion.Euler(0, angle, 0);

            // Calculate move direction based on camera-facing angle
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            if (!sliding)
            {
                if (OnSlope())
                {
                    // Move relative to slope while on one
                    moveDir = GetSlopeMoveDirection().normalized;
                }
                else
                {
                    moveDir = moveDir.normalized;
                }

                // Calculate the target velocity based on desired direction and current speed
                Vector3 horizVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
                float currentSpeed = horizVel.magnitude;
                
                Vector3 targetVelocity = moveDir * Mathf.Max(speed, currentSpeed);  // Maintain current speed or target speed, whichever is higher

                // Calculate the force needed to gradually steer towards the target velocity
                Vector3 velChange = targetVelocity - currentVel;

                // Apply a consistent steering force without abrupt acceleration, clamped by maxForce
                velChange = Vector3.ClampMagnitude(velChange, maxForce);

                // Ignore y forces unless on a slope to prevent "gliding" behavior in the air
                if (!OnSlope())
                {
                    velChange = new Vector3(velChange.x, 0, velChange.z);
                }

                // Offset drag on ground
                if (grounded)
                {
                    velChange *= rb.linearDamping;
                }

                // half force when above target speed
                if(currentSpeed > airSpeed)
                {
                    velChange *= maxAirSpeedMod;
                }

                // Apply the force for smooth movement and direction control
                rb.AddForce(velChange * rb.mass * (Time.deltaTime * expectedFramerate), ForceMode.Force);
            }
        }
        else
        {
            moveDir = Vector3.zero;
        }
    }

    private void Jump()
    {
        // Reset y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        // Calculate base force
        Vector3 jumpForce = Vector3.up * jumpPower * rb.mass;

        // If on a slope, jump angle should be relative to the slope's angle
        if (OnSlope() || OnSteepSlope())
        {
            jumpForce = GetSlopeNormal().normalized * jumpPower * rb.mass;
        }

        if(move.x > 0f || move.y > 0f)
        {
            // Add some extra force in the direction the player is facing
            jumpForce += transform.forward * rb.mass * jumpSpeed;
        }

        // Apply force
        rb.AddForce(jumpForce, ForceMode.Impulse);

        // Play sfx
        playerSounds.PlayJumpSfx();
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void Dash()
    {
        if (dashCooldownTimer >= dashCooldownTime)
        {
            // Reduce dash power when over target speed
            float dashPowerProportional = dashPower;
            if(rb.linearVelocity.magnitude > speed)
            {
                dashPowerProportional *= speed / (2 * rb.linearVelocity.magnitude);
            }

            // Add force in the direction the player is facing
            Vector3 dashForce = dashPowerProportional * rb.mass * transform.forward;
            rb.AddForce(dashForce, ForceMode.Impulse);

            // Reset timer
            dashCooldownTimer = 0f;
        }
    }

    public void SetGrounded(bool state)
    {
        grounded = state;
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(rb.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.4f))
        {
            float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return slopeAngle < maxSlopeAngle && slopeAngle != 0;
        }

        return false;
    }

    public bool OnSteepSlope()
    {
        if (Physics.Raycast(rb.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.5f))
        {
            float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return slopeAngle > maxSlopeAngle && slopeAngle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDir, slopeHit.normal).normalized;
    }

    private Vector3 GetSlopeNormal()
    {
        if (Physics.Raycast(rb.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.5f))
        {
            return slopeHit.normal;
        } else { return Vector3.zero; }
    }

    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(rb.position, Vector3.down * (playerHeight * 0.5f + 0.5f), Color.green);
    }
}
