using UnityEngine;

public class Climbing : MonoBehaviour
{

    [Header("References")]
    public Transform orientation;
    public Rigidbody rb;
    public PlayerMovement pm;
    public LayerMask whatIsWall;
    public LayerMask whatIsWallInfinite;

    [Header("Climbing")]
    public float climbSpeed;
    public float maxClimbTime;
    private float climbTimer;

    [Header("Climb Jump")]
    public float climbJumpUpForce;
    public float climbJumpBackForce;

    public int climbJumps;
    private int climbJumpsLeft;

    public int spamJumps;
    private int spamJumpsLeft;

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

    [Header("Exiting")]
    public bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;
    private bool infiniteWall;

    void Update()
    {
        WallCheck();
        StateMachine(); 
    }

    private void FixedUpdate()
    {
        if (pm.climbing && !exitingWall) ClimbingMovement();
    }

    private void StateMachine()
    {
        // State 1 - Climbing
        if (wallFront && pm.move.y > 0 && wallLookAngle < maxWallLookAngle && !exitingWall)
        {
            if (!pm.climbing && (climbTimer > 0 || infiniteWall)) StartClimbing();

            // timer
            if (climbTimer > 0 && !infiniteWall) climbTimer -= Time.deltaTime;
            if (climbTimer <= 0 && !infiniteWall) StopClimbing();
        }

        // State 2 - Exiting Wall
        else if (exitingWall)
        {
            if (pm.climbing) StopClimbing();

            if (exitWallTimer > 0) exitWallTimer -= Time.deltaTime;
            if (exitWallTimer <= 0)
            {
                spamJumpsLeft = spamJumps;
                exitingWall = false;
            }
        }

        // State 3 - None
        else
        {
            if (pm.climbing) StopClimbing();
        }

        if (wallFront && pm.jumpInput > 0 && climbJumpsLeft > 0) ClimbJump();
    }

    private void WallCheck()
    {
        //check if it hits any wall
        wallFront = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallHit, detectionLength, whatIsWall | whatIsWallInfinite);

        if (wallFront) // hits a wall
        {
            wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);

            // infinite layer
            infiniteWall = (1 << frontWallHit.collider.gameObject.layer & whatIsWallInfinite) != 0;

          
            bool newWall = frontWallHit.transform != lastWall || Mathf.Abs(Vector3.Angle(lastWallNormal, frontWallHit.normal)) > minWallNormalAngleChange;

           
            if (newWall || pm.grounded)
            {
                climbTimer = maxClimbTime;
                climbJumpsLeft = climbJumps;
            }
        }
        else
        {
          
            infiniteWall = false;
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
        if (exitingWall && spamJumpsLeft > 0)
        {
            spamJumpsLeft--;
        } else if(exitingWall)
        {
            return;
        }

        Vector3 forceToApply = transform.up * climbJumpUpForce * rb.mass + frontWallHit.normal * climbJumpBackForce * rb.mass;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);

        climbJumpsLeft--;

        exitingWall = true;
        exitWallTimer = exitWallTime;
    }
}
