using System;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerMovement pm;
    private Rigidbody rb;
    private Animator anim;
    private PlayerAudio stepAudio;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();
        stepAudio = GetComponent<PlayerAudio>();
    }

    void Update()
    {
        if (pm.state == PlayerMovement.MovementState.walking && rb.linearVelocity.magnitude > 1)
        {
            PlayerAudio.footstepTypes ground;
            Enum.TryParse(groundType(), out ground);

            stepAudio.setFootstepType(ground);
            anim.speed = rb.linearVelocity.magnitude/ 9f;
            anim.SetBool("isWalking", true);
        } else
        {
            anim.speed = 1;
            anim.SetBool("isWalking", false);
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
