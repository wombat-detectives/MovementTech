using UnityEngine;
using Debug = UnityEngine.Debug;
public class PlayerAnimations : MonoBehaviour
{
    private Animator animator;
    [HideInInspector] public string currentAnimation = "";

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlaySlideAnimation()
    {
        if (currentAnimation != "Slide")
        {
            animator.Play("Slide", 1);
            currentAnimation = "Slide";
        }
    }

    public void PlayIdleAnimation()
    {
        if (currentAnimation != "Idle" && currentAnimation != "Jump")
        {
            animator.CrossFade("Idle", 0.2f, 1);
            currentAnimation = "Idle";
        }
    }
    public void PlayIdleCrouchAnimation()
    {

    }

    public void PlayCrouchWalkAnimation()
    {

    }
    public void PlayWallrunAnimation()
    {
        if (currentAnimation != "Climb")
        {
            animator.Play("Climb", 1);
            currentAnimation = "Climb";
        }
    }
    public void PlayClimbAnimation()
    {
        if (currentAnimation != "Climb")
        {
            animator.Play("Climb", 1);
            currentAnimation = "Climb";
        }
    }

    public void PlayAirAnimation()
    {
        PlayJumpAnimation();
    }
    public void PlayRunAnimation()
    {
        Debug.Log("Run animation called");
        if (currentAnimation == "Run")
        {
            Debug.Log("Canceling run");
        }
        if (currentAnimation != "Run" && currentAnimation != "Jump" && currentAnimation != "Lunge")
        {
            Debug.Log("Play Run animation");
            animator.CrossFade("Run", 0.2f, 1);
            currentAnimation = "Run";
        }
    }

    public void PlayDashAnimation()
    {
        if (currentAnimation != "Dash")
        {
            animator.CrossFade("Dash", 0.2f, 2);
            currentAnimation = "Dash";
        }
    }
    public void PlayLungeAnimation()
    {
        if (currentAnimation != "Lunge")
        {
            animator.CrossFade("Lunge", 0.2f, 1);
            currentAnimation = "Lunge";
        }
    }
    public void PlayJumpAnimation()
    {
        if (currentAnimation != "Jump")
        {
            animator.CrossFade("Jump", 0.2f, 1);
            currentAnimation = "Jump";
        }
    }

    public void PlayLandJumpAnimation()
    {
        if (currentAnimation != "LandJump")
        {
            animator.CrossFade("LandJump", 0.2f, 2);
            currentAnimation = "LandJump";
        }
    }

    public void PlayWallKickAnimation()
    {
        if (currentAnimation != "WallKick")
        {
            animator.CrossFade("WallKick", 0.2f, 1);
            currentAnimation = "WallKick";
        }
    }
}
