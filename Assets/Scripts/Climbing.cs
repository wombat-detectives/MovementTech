using UnityEngine;

public class Climbing : MonoBehaviour
{

    [Header("References")]
    public Transform orientation;
    public Rigidbody rb;
    public PlayerMovement pm;
    public LayerMask whatIsWall;

    [Header("Climbing")]
    public float climbSpeed;
    public float maxClimbTime;
    private float climbTimer;

    [Header("Climb Jump")]
    public float climbJumpUpForce;
    public float climbJumpBackForce;

    public int climbJumps;
    private int climbJumpsLeft;

    [Header("Detection")]
    public float detectionLength;
    public float sphereCastRadius;
    public float maxWallLookAngle;
    private float wallLookAngle;

    private RaycastHit frontWallHit;
    private bool wallFront;

    private Transform lastWall;
    private Vector3 lastWallNormal;
    public float minWallNormalAngleChange;

    void Update()
    {
        WallCheck();
        StateMachine();

        if (pm.climbing) ClimbingMovement();
    }

    private void StateMachine()
    {
        // State 1 - Climbing
        if(wallFront && pm.move.y > 0 && wallLookAngle < maxWallLookAngle)
        {
            if(!pm.climbing && climbTimer > 0) StartClimbing();

            // timer
            if(climbTimer >= 0) climbTimer -= Time.deltaTime;
            if(climbTimer < 0) StopClimbing();
        }

        // State 3 - None
        else
        {
            if(pm.climbing) StopClimbing();
        }

        if (wallFront && pm.jumpInput > 0 && climbJumpsLeft > 0) ClimbJump();
    }

    private void WallCheck()
    {
        wallFront = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallHit, detectionLength, whatIsWall);
        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);

        bool newWall = frontWallHit.transform != lastWall || Mathf.Abs(Vector3.Angle(lastWallNormal, frontWallHit.normal)) > minWallNormalAngleChange;

        if ((wallFront && newWall) || pm.grounded)
        {
            climbTimer = maxClimbTime;
            climbJumpsLeft = climbJumps;
        }
    }

    private void StartClimbing()
    {
        pm.climbing = true;

        lastWall = frontWallHit.transform;
        lastWallNormal = frontWallHit.normal;
    }

    private void ClimbingMovement()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, climbSpeed, rb.linearVelocity.z);
    }

    private void StopClimbing()
    {
        pm.climbing = false;
    }

    private void ClimbJump()
    {
        Vector3 forceToApply = transform.up * climbJumpUpForce * rb.mass + frontWallHit.normal * climbJumpBackForce * rb.mass;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);

        climbJumpsLeft--;
    }
}
