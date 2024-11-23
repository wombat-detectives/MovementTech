using UnityEngine;

public class AnimationStartController : MonoBehaviour
{
    public float startPoint;
    public float animSpeed;
    public string animationName;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = animSpeed;
        anim.Play(animationName, 0, startPoint);

    }
}
