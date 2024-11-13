using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public float maxWallRunTime;
    private float wallRunTimer;
    public bool useGravity;
    public float gravityCounterForce;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;

    public float minWallNormalAngleChange;
    private Transform lastWall;
    private Vector3 lastWallNormal;

    [Header("Exiting")]
    private bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;


    [Header("References")]
    public Transform orientation;
    private Rigidbody rb;
    private PlayerMovement pm;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();

    }

    private void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if (pm.wallrunning)
        {
            WallrunningMovement();
        }
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);

        // Stamina system, modified from climbing script
        bool wallNear = wallLeft || wallRight;
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        RaycastHit currWallHit = wallRight ? rightWallHit : leftWallHit;

        bool newWall = currWallHit.transform != lastWall || Mathf.Abs(Vector3.Angle(lastWallNormal, wallNormal)) > minWallNormalAngleChange;

        if ((wallNear && newWall) || pm.grounded)
        {
            wallRunTimer = maxWallRunTime;
        }
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine()
    {

        // State 1 - Wallrunning
        if ((wallLeft || wallRight) && pm.move.y > 0 && AboveGround() && !exitingWall && wallRunTimer > 0)
        {
            if (!pm.wallrunning)
                StartWallRun();

            // timer
            if (wallRunTimer > 0)
                wallRunTimer -= Time.deltaTime;

            if(wallRunTimer <= 0 && pm.wallrunning)
            {
                exitingWall = true;
                exitWallTimer = exitWallTime;
            }

            // wall jump
            if (pm.jumpInput > 0) WallJump();
        }

        // State 2 - Exiting
        else if (exitingWall)
        {
            if (pm.wallrunning)
                StopWallRun();

            if (exitWallTimer > 0)
                exitWallTimer -= Time.deltaTime;

            if (exitWallTimer <= 0)
                exitingWall = false;
        }

        // State 3 - None
        else
        {
            if (pm.wallrunning)
                StopWallRun();
        }

        
    }

    private void StartWallRun()
    {
        pm.wallrunning = true;

        lastWall = wallRight ? rightWallHit.transform : leftWallHit.transform;
        lastWallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
    }

    private void WallrunningMovement()
    {
        rb.useGravity = useGravity;
        

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        // forward force
        rb.AddForce(wallForward * wallRunForce * (Time.deltaTime * pm.expectedFramerate), ForceMode.Force);

        // push to wall force
        if(!(wallLeft && pm.move.x > 0) && !(wallRight && pm.move.x < 0))
            rb.AddForce(-wallNormal * 100 * (Time.deltaTime * pm.expectedFramerate), ForceMode.Force);

        // weaken gravity
        if (useGravity)
            rb.AddForce(transform.up * gravityCounterForce * (Time.deltaTime * pm.expectedFramerate));
    }

    private void StopWallRun()
    {
        pm.wallrunning = false;
        rb.useGravity = true;
    }

    private void WallJump()
    {
        // enter exiting wall state
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = wallRight ? rightWallHit.normal: leftWallHit.normal;

        Vector3 forceToApply = transform.up * wallJumpUpForce * rb.mass + wallNormal * wallJumpSideForce * rb.mass;

        // add force
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }

}
