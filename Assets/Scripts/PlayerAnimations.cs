using UnityEngine;
using System.Collections; // Required for IEnumerator and Coroutines
using Debug = UnityEngine.Debug;

public class PlayerAnimations : MonoBehaviour
{
    private Animator animator;
    [HideInInspector] public string currentAnimation = "";
    private bool isKeyPickupAnimationPlaying = false; // Lock for key pickup animation

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlaySlideAnimation()
    {
        if (!isKeyPickupAnimationPlaying && currentAnimation != "Slide")
        {
            animator.Play("Slide", 1);
            currentAnimation = "Slide";
        }
    }

    public void PlayKeyPickupAnimation()
    {
        if (!isKeyPickupAnimationPlaying)
        {
            isKeyPickupAnimationPlaying = true; // Lock animation state
            animator.Play("KeyPickupAnim", 1);

            // Start a coroutine to unlock after the animation ends
            StartCoroutine(UnlockKeyPickupAnimation());
        }
    }

    private IEnumerator UnlockKeyPickupAnimation()
    {
        // Wait until the KeyPickupAnim animation finishes
        yield return new WaitUntil(() =>
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(1); // Layer 1
            return stateInfo.IsName("KeyPickupAnim") && stateInfo.normalizedTime >= 1.0f;
        });

        isKeyPickupAnimationPlaying = false; // Unlock after animation
    }

    public void PlayIdleAnimation()
    {
        if (!isKeyPickupAnimationPlaying && currentAnimation != "Idle" && currentAnimation != "Jump")
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

    public void PlayWallrunAnimation()
    {
        if (!isKeyPickupAnimationPlaying && currentAnimation != "Wallrun")
        {
            animator.Play("Wallrun", 1);
            currentAnimation = "Wallrun";
        }
    }

    public void PlayClimbAnimation()
    {
        if (!isKeyPickupAnimationPlaying && currentAnimation != "Climb")
        {
            animator.Play("Climb", 1);
            currentAnimation = "Climb";
        }
    }

    public void PlayAirAnimation()
    {
        if (!isKeyPickupAnimationPlaying && currentAnimation != "Air")
        {
            animator.CrossFade("Air", 0.2f, 1);
            currentAnimation = "Air";
        }
    }

    public void PlayRunAnimation()
    {
        if (!isKeyPickupAnimationPlaying && currentAnimation != "Run")
        {
            Debug.Log("Run animation called");
            animator.CrossFade("Run", 0.2f, 1);
            currentAnimation = "Run";
        }
    }

    public void PlayFallShortAnimation()
    {
        if (!isKeyPickupAnimationPlaying && currentAnimation != "FallShort")
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(1); // Layer 1
            if (stateInfo.normalizedTime >= 1.0f && !stateInfo.IsName("FallShort"))
            {
                animator.CrossFade("FallShort", 0.2f, 1); // Play FallShort after current animation finishes
                currentAnimation = "FallShort";
            }
        }
    }

    public void PlayFallLongAnimation()
    {
        if (!isKeyPickupAnimationPlaying && currentAnimation != "FallLong")
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(1); // Layer 1
            if (stateInfo.normalizedTime >= 1.0f && !stateInfo.IsName("FallLong"))
            {
                animator.CrossFade("FallLong", 0.2f, 1); // Play FallLong after current animation finishes
                currentAnimation = "FallLong";
            }
        }
    }

    public void PlayDashAnimation()
    {
        if (!isKeyPickupAnimationPlaying && currentAnimation != "Dash")
        {
            animator.CrossFade("Dash", 0.2f, 2);
            currentAnimation = "Dash";
        }
    }

    public void PlayLungeAnimation()
    {
        if (!isKeyPickupAnimationPlaying && currentAnimation != "Lunge")
        {
            animator.CrossFade("Lunge", 0.2f, 1);
            currentAnimation = "Lunge";
        }
    }

    public void PlayJumpAnimation()
    {
        if (!isKeyPickupAnimationPlaying && currentAnimation != "Jump")
        {
            animator.Play("Jump", 1);
            currentAnimation = "Jump";
        }
    }

    public void PlayLandJumpAnimation()
    {
        if (!isKeyPickupAnimationPlaying && currentAnimation != "LandJump")
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
        if (!isKeyPickupAnimationPlaying && currentAnimation != "WallKick")
        {
            animator.CrossFade("WallKick", 0.2f, 1);
            currentAnimation = "WallKick";
        }
    }
}
