using System;
using UnityEngine;

public class PlayerSFXManager : MonoBehaviour
{
    private PlayerMovement pm;
    private Rigidbody rb;
    private PlayerAnimations animations;
    private FootstepAudio stepAudio;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        animations = GetComponentInChildren<PlayerAnimations>();
        stepAudio = GetComponentInChildren<FootstepAudio>();
    }

    void Update()
    {
        if (pm.grounded)
        {
            FootstepAudio.footstepTypes ground;
            Enum.TryParse(groundType(), out ground);

            stepAudio.setFootstepType(ground);
        }
    }

    private string groundType()
    {
        RaycastHit hit;
        Physics.Raycast(rb.position, Vector3.down, out hit, pm.playerHeight * 0.5f + 0.3f, pm.whatIsGround);

        if (hit.collider != null)
        {
            return hit.collider.tag;
        }
        else
            return null;
    }

}
