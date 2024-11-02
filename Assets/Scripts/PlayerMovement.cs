using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 move, look;
    private Vector3 moveDir;
    private float turnSmoothVel;
    private float expectedFramerate = 60f;

    [Header("Setup Fields")]
    [SerializeField] private Transform PlayerCamera;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform PlayerModel;
    public float turnSmoothTime = 0.1f;
    [Space]
    [Header("Lateral Movement")]
    public float speed, maxForce;
    public float dashPower, dashCooldownTime;
    private float dashCooldownCounter;

    [Header("Jumping")]
    [SerializeField] private float jumpPower = 500f;
    [SerializeField] private float jumpSpeed = 1f;
    private bool isGrounded;

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
        //  Dash counter
        if(dashCooldownCounter > 0)
        {
            dashCooldownCounter -= Time.deltaTime;
            dashCooldownCounter = Mathf.Clamp(dashCooldownCounter, 0f, Mathf.Infinity);
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
        dashDisplay.text = "Dash: " + string.Format("{0:0.00}", dashCooldownCounter);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    // Not being used rn
    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
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

        // Face direction of movement
        if (targetDir.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg + PlayerCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(rb.rotation.eulerAngles.y, targetAngle, ref turnSmoothVel, turnSmoothTime);
            rb.rotation = Quaternion.Euler(0, angle, 0);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            Vector3 velChange;
            if (OnSlope())
            {
                moveDir = GetSlopeMoveDirection() * speed;

                // Calculate Forces
                velChange = moveDir - currentVel;
                velChange = new Vector3(velChange.x, velChange.y, velChange.z);
            }
            else
            {
                moveDir = moveDir.normalized * speed;

                // Calculate Forces
                velChange = moveDir - currentVel;
                velChange = new Vector3(velChange.x, 0, velChange.z);
            }

            // Limit Force for safety
            Vector3.ClampMagnitude(velChange, maxForce);

            rb.AddForce(velChange * rb.mass * (Time.deltaTime * expectedFramerate), ForceMode.Force);
        } else
        {
            moveDir = Vector3.zero;
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            // Calculate base force
            Vector3 jumpForce = Vector3.up * jumpPower * rb.mass;

            // If on a slope, jump angle should be relative to the slope's angle
            if (OnSlope())
            {
                jumpForce = GetSlopeNormal().normalized * jumpPower * rb.mass;
            }

            // Add some extra force in the direction the player is facing
            jumpForce += transform.forward * rb.mass * jumpSpeed;

            // Apply force
            rb.AddForce(jumpForce, ForceMode.Impulse);
        }
    }

    private void Dash()
    {
        if (dashCooldownCounter <= 0)
        {
            // Add force in the direction the player is facing
            Vector3 dashForce = dashPower * rb.mass * transform.forward;
            rb.AddForce(dashForce, ForceMode.Impulse);

            // Reset timer
            dashCooldownCounter = dashCooldownTime;
        }
    }

    public void SetGrounded(bool state)
    {
        isGrounded = state;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(rb.position, Vector3.down, out slopeHit, slopeRaycastSize))
        {
            float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return slopeAngle < maxSlopeAngle && slopeAngle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
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
