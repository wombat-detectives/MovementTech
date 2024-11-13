using System;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerMovement pm;
    private Rigidbody rb;
    private Animator anim;
    private PlayerAudio audio;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();
        audio = GetComponent<PlayerAudio>();
    }

    void Update()
    {
        if (pm.state == PlayerMovement.MovementState.walking && rb.linearVelocity.magnitude > 1)
        {
            PlayerAudio.footstepTypes ground;
            Enum.TryParse(groundType(), out ground);

            audio.setFootstepType(ground);
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
            Debug.Log(hit.collider.tag);
            return hit.collider.tag;
        }
        else
            return null;
    }

}
