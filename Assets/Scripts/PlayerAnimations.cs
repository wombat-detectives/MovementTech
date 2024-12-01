using UnityEngine;
using Debug = UnityEngine.Debug;
using System.Collections;
public class PlayerAnimations : MonoBehaviour
{
    private Animator animator;
    [HideInInspector] public string currentAnimation = "";
    private bool isKeyPickupActive = false; // Flag to track if KeyPickup animation is active

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlaySlideAnimation()
    {
        if (currentAnimation != "Slide" && !isKeyPickupActive)
        {
            animator.Play("Slide", 1);
            currentAnimation = "Slide";
        }
    }

    public void PlayKeyPickupAnimation()
    {
        // Set the flag to true and play the KeyPickupAnim
        isKeyPickupActive = true;
        animator.Play("KeyPickupAnim", 1);
        currentAnimation = "KeyPickupAnim";
    
    }

   

    public void PlayIdleAnimation()
    {
        if (currentAnimation != "Idle" && currentAnimation != "Jump" && !isKeyPickupActive)
        {
            if (currentAnimation == "FallShort" || currentAnimation == "FallLong")
            {
                animator.CrossFade("Idle", 0.1f, 1); // Shorter crossfade for falling animations
            }
            else
            {
                animator.CrossFade("Idle", 0.2f, 1); // Default crossfade for other transitions
            }

            currentAnimation = "Idle";
        }
    }

    public void PlayIdleCrouchAnimation()
    {
        if (!isKeyPickupActive)
        {
            // Implement crouch logic
        }
    }

    public void PlayCrouchWalkAnimation()
    {
        if (!isKeyPickupActive)
        {
            // Implement crouch walk logic
        }
    }

    public void PlayWallrunAnimation()
    {
        if (currentAnimation != "Climb" && !isKeyPickupActive)
        {
            animator.Play("Climb", 1);
            currentAnimation = "Climb";
        }
    }

    public void PlayClimbAnimation()
    {
        if (currentAnimation != "Climb" && !isKeyPickupActive)
        {
            animator.Play("Climb", 1);
            currentAnimation = "Climb";
        }
    }

    public void PlayFallShortAnimation()
    {
        if (currentAnimation != "FallShort" && !isKeyPickupActive)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(1); // Layer 1
            if (stateInfo.normalizedTime >= 1.0f && !stateInfo.IsName("FallShort"))
            {
                animator.CrossFade("FallShort", 0.2f, 1);
                currentAnimation = "FallShort";
            }
        }
    }

    public void PlayFallLongAnimation()
    {
        if (currentAnimation != "FallLong" && !isKeyPickupActive)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(1); // Layer 1
            if (stateInfo.normalizedTime >= 1.0f && !stateInfo.IsName("FallLong"))
            {
                animator.CrossFade("FallLong", 0.2f, 1);
                currentAnimation = "FallLong";
            }
        }
    }

    public void PlayAirAnimation()
    {
        if (!isKeyPickupActive)
        {
            PlayJumpAnimation();
        }
    }

    public void PlayRunAnimation()
    {
        if (!isKeyPickupActive && currentAnimation != "Run" && currentAnimation != "Jump" && currentAnimation != "Lunge")
        {
            animator.CrossFade("Run", 0.2f, 1);
            currentAnimation = "Run";
        }
    }

    public void PlayDashAnimation()
    {
        if (!isKeyPickupActive && currentAnimation != "Dash")
        {
            animator.CrossFade("Dash", 0.2f, 2);
            currentAnimation = "Dash";
        }
    }

    public void PlayLungeAnimation()
    {
        if (!isKeyPickupActive && currentAnimation != "Lunge")
        {
            animator.CrossFade("Lunge", 0.2f, 1);
            currentAnimation = "Lunge";
        }
    }

    public void PlayJumpAnimation()
    {
        if (!isKeyPickupActive && currentAnimation != "Jump")
        {
            animator.Play("Jump", 1);
            currentAnimation = "Jump";
        }
    }

    public void PlayLandJumpAnimation()
    {
        if (!isKeyPickupActive && currentAnimation != "LandJump")
        {
            if (currentAnimation == "FallShort" || currentAnimation == "FallLong")
            {
                animator.CrossFade("LandJump", 0.05f, 2);
            }
            else
            {
                animator.CrossFade("LandJump", 0.2f, 2);
            }

            currentAnimation = "LandJump";
        }
    }

    public void PlayWallKickAnimation()
    {
        if (!isKeyPickupActive && currentAnimation != "WallKick")
        {
            animator.CrossFade("WallKick", 0.2f, 1);
            currentAnimation = "WallKick";
        }
    }
}
