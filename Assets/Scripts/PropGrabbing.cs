using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PropGrabbing : MonoBehaviour
{
    [Header ("Pickup")]
    public string whatIsProp;
    public float grabRange;

    [Header("Holding")]
    public Vector3 heldPropPosition;
    public Quaternion heldPropRotation;

    private GameObject heldProp;
    private Rigidbody heldPropRB;
    private bool isHoldingProp;

    [Header("Throw / Drop")]
    public float propThrowForce;
    public Vector3 dropPropPosition;
    private Rigidbody playerRB;

    [Header("Jank Launch")]
    public float launchForce = 25f;
    private PlayerMovement pm;

    private void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    public void OnLClick(InputAction.CallbackContext context)
    {
        if (context.performed)
            TryThrow();
    }

    public void OnRClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TryPickupDrop();
        }
            
    }

    private void TryThrow()
    {
        // Allow throwing of prop if one is held
        if (isHoldingProp)
        {
            ThrowProp();
        }
    }

    private void TryPickupDrop()
    {

        //Pick up prop if one isn't held and a prop is within range
        if (!isHoldingProp && PropCheck())
        {
            PickupProp();
        } 
        //Drop prop if one is held
        else if (isHoldingProp)
        {
            // normal drop
            if(!pm.sliding)
                DropProp();
            // perform jank launch while sliding
            else
            {
                DropProp(true);
                JankLaunch();
            }
            
        }
    }

    void Update()
    {
        //Lock prop position while held
        if (isHoldingProp)
        {
            HoldingProp();
        }
    }

    private bool PropCheck()
    {
        var result = Physics.OverlapSphere(transform.position, grabRange);

        foreach (var prop in result)
        {
            if (prop.transform.CompareTag(whatIsProp))
            {
                return true;
            }
        }
        return false;
    }

    private void PickupProp()
    {
        //Check for all props within grab range
        var result = Physics.OverlapSphere(transform.position, grabRange);

        List<Collider> props = new List<Collider>();
        foreach (var prop in result)
        {
            if (prop.transform.CompareTag(whatIsProp))
            {
                props.Add(prop);
            }
        }

        //Get nearest prop
        GameObject closestProp = null;
        float closestDistance = grabRange + 1;
        foreach (var prop in props)
        {
            if (Vector3.Distance(transform.position, prop.transform.position) < closestDistance)
            {
                closestDistance = Vector3.Distance(transform.position, prop.transform.position);
                closestProp = prop.gameObject;
            }
        }

        //hold nearest prop (if found)
        if (closestProp != null)
        {
            heldProp = closestProp;
            heldPropRB = heldProp.GetComponent<Rigidbody>();
        }

        //Assign heldProp as child of player
        heldProp.transform.parent = transform;

        //Make prop appear above player
        heldProp.transform.localPosition = heldPropPosition;

        if (heldProp != null)
        {
            isHoldingProp = true;
        }

        //Play pickup sfx
        /*audioSrc.clip = pickupSFX;
        audioSrc.Play();*/
    }

    private void HoldingProp()
    {
        heldProp.transform.SetLocalPositionAndRotation(heldPropPosition, heldPropRotation);
    }

    private void ThrowProp()
    {
        //if a prop is held, give it velocity in direction player is facing
        isHoldingProp = false;
        heldProp.transform.parent = null;
        heldPropRB.linearVelocity = playerRB.linearVelocity;
        heldPropRB.angularVelocity = playerRB.angularVelocity;
        heldPropRB.AddForce(transform.forward * propThrowForce * heldPropRB.mass, ForceMode.Impulse);

        heldProp = null;
        heldPropRB = null;

        //Play throw sfx
        /*audioSrc.clip = throwSFX;
        audioSrc.Play();*/
    }

    private void DropProp(bool jank = false)
    {
        //if a prop is held, drop it in front of the player
        isHoldingProp = false;
        if (!jank)
            heldProp.transform.localPosition = dropPropPosition;
        else
            heldProp.transform.localPosition = Vector3.zero;
        heldPropRB.linearVelocity = Vector3.zero;
        heldPropRB.angularVelocity = playerRB.angularVelocity;
        heldProp.transform.parent = null;

        heldProp = null;
        heldPropRB = null;
    }

    private void JankLaunch()
    {
        playerRB.AddForce(Vector3.up * playerRB.mass * launchForce, ForceMode.Impulse);
    }
}
