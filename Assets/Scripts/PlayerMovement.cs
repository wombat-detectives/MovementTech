using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 move;
    private Vector3 moveDir;
    private float turnSmoothVel;
    private float expectedFramerate = 60f;
    private Sliding slideController;

    [Header("Setup Fields")]
    [SerializeField] private Transform PlayerCamera;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform PlayerModel;
    public float turnSmoothTime = 0.1f;
    [Space]
    [Header("Lateral Movement")]
    public float speed, maxForce, airControlMod;
    public float dashPower, dashCooldownTime;
    private float dashCooldownTimer;

    [Header("Jumping")]
    [SerializeField] private float jumpPower = 500f;
    [SerializeField] private float jumpSpeed = 1f;
    [SerializeField] private float jumpCooldown = .15f;
    private bool isGrounded;
    private float jumpCooldownTimer;

    [Header("Slope Handling")]
    public float maxSlopeAngle = 50f;
    private RaycastHit slopeHit;
    [SerializeField] private float slopeRaycastSize = 2f;

    [Header("Stat Tracking")]
    public TextMeshProUGUI velocityDisplay;
    public TextMeshProUGUI dashDisplay;

    void Update()
    {
        // Movement
        MovePlayer();

        //  Dash timer
        if(dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
            dashCooldownTimer = Mathf.Clamp(dashCooldownTimer, 0f, Mathf.Infinity);
        }

        // Jump timer
        if(jumpCooldownTimer > 0)
        {
            jumpCooldownTimer -= Time.deltaTime;
            jumpCooldownTimer = Mathf.Clamp(jumpCooldownTimer, 0f, Mathf.Infinity);
        }

        // Cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursor();
        }

        // UI
        UpdateUI();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        slideController = GetComponent<Sliding>();
    }

    private void ToggleCursor()
    {
        if(Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
        } else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void UpdateUI()
    {
        velocityDisplay.text = "Velocity: " + string.Format("{0:000.00}", rb.linearVelocity.magnitude);
        dashDisplay.text = "Dash: " + string.Format("{0:0.00}", dashCooldownTimer);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Jump();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        Dash();
    }

    private void MovePlayer()
    {
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

            if (!slideController.sliding)
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
                float currentSpeed = currentVel.magnitude;
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

                // Reduce control in the air
                if (!isGrounded)
                {
                    velChange *= airControlMod;
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
        if (isGrounded && jumpCooldownTimer <= 0)
        {
            // Calculate base force
            Vector3 jumpForce = Vector3.up * jumpPower * rb.mass;

            // If on a slope, jump angle should be relative to the slope's angle
            if (OnSlope())
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

            // Reset timer
            jumpCooldownTimer = jumpCooldown;
        }
    }

    private void Dash()
    {
        if (dashCooldownTimer <= 0)
        {
            // Add force in the direction the player is facing
            Vector3 dashForce = dashPower * rb.mass * transform.forward;
            rb.AddForce(dashForce, ForceMode.Impulse);

            // Reset timer
            dashCooldownTimer = dashCooldownTime;
        }
    }

    public void SetGrounded(bool state)
    {
        isGrounded = state;
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(rb.position, Vector3.down, out slopeHit, slopeRaycastSize))
        {
            float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return slopeAngle < maxSlopeAngle && slopeAngle != 0;
        }

        return false;
    }

    public bool OnSteepSlope()
    {
        if (Physics.Raycast(rb.position, Vector3.down, out slopeHit, slopeRaycastSize))
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
        if (Physics.Raycast(rb.position, Vector3.down, out slopeHit, slopeRaycastSize))
        {
            return slopeHit.normal;
        } else { return Vector3.zero; }
    }

    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(rb.position, Vector3.down * slopeRaycastSize, Color.green);
    }
}
