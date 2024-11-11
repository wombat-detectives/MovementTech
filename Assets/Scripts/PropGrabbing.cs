using NUnit.Framework;
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

    private void Start()
    {
        playerRB = GetComponent<Rigidbody>();
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
            DropProp();
        }
    }

    void Update()
    {
        //Lock prop position above player while held
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
        heldPropRB. angularVelocity = playerRB.angularVelocity;
        heldPropRB.AddForce(transform.forward * propThrowForce, ForceMode.Impulse);

        heldProp = null;
        heldPropRB = null;

        //Play throw sfx
        /*audioSrc.clip = throwSFX;
        audioSrc.Play();*/
    }

    private void DropProp()
    {
        //if a prop is held, drop it in front of the player
        isHoldingProp = false;
        heldProp.transform.localPosition = dropPropPosition;
        heldPropRB.linearVelocity = Vector3.zero;
        heldProp.transform.parent = null;

        heldProp = null;
        heldPropRB = null;
    }
}
