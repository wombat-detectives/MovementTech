using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Sliding : MonoBehaviour
{
    [Header("References")]
    public Transform playerObj;
    public Transform orientation;
    private Rigidbody rb;
    private Collider coll;
    private PlayerMovement pm;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    public float slideDownForce;
    private float slideTimer;

    public float slideFriction = 0.1f;
    private float startFriction;

    public float slideYScale;
    private float startYScale;

    [Header("Input")]
    private bool isCrouching;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();

        startYScale = playerObj.localScale.y;
        coll = playerObj.GetComponent<Collider>();
        startFriction = coll.material.dynamicFriction;
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        float val = context.ReadValue<float>();
        isCrouching = val > 0;

        if (pm.move.x != 0 || pm.move.y != 0)
        {
            StartSlide();
        }
    }

    private void Update()
    {
        if(!isCrouching && pm.sliding)
        {
            StopSlide();
        }    
    }

    private void FixedUpdate()
    {
        if (pm.sliding)
        {
            SlidingMovement();
        }
    }

    private void StartSlide()
    {
        pm.sliding = true;

        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        rb.AddForce(Vector3.down * slideDownForce * rb.mass, ForceMode.Impulse);

        slideTimer = maxSlideTime;

        // Reduce friction
        coll.material.dynamicFriction = slideFriction;
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * pm.move.y + orientation.right * pm.move.x;

        // Normal Slide
        if ((!pm.OnSlope() && !pm.OnSteepSlope()) || rb.linearVelocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce * 0.1f * rb.mass, ForceMode.Force);
        }
        // Sliding down a slope
        else
        {
            rb.AddForce(pm.GetSlopeMoveDirection() * slideForce * rb.mass, ForceMode.Force);
        }

        // Apply downforce
        rb.AddForce(Vector3.down * slideDownForce * rb.mass, ForceMode.Force);

        // Reduce timer if under normal speed
        if(rb.linearVelocity.magnitude < 14f)
        {
            slideTimer -= Time.deltaTime;
        }

        if (slideTimer <= 0)
        {
            StopSlide();
        }
    }

    private void StopSlide()
    {
        pm.sliding = false;

        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);

        // Reset friction
        coll.material.dynamicFriction = startFriction;
    }
}
