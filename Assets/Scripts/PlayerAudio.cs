using UnityEngine;

public class PlayerAudio : MonoBehaviour
{

    public float volume;

    [Header("Footsteps")]
    public AudioClip[] grassFootsteps;
    public AudioClip[] brickFootsteps;
    public AudioClip[] propFootsteps;

    public enum footstepTypes
    {
        Grass,
        Brick,
        Prop,
        Untagged
    }

    private AudioClip[] activeFootsteps;

    public void playFootstep()
    {
        //SFXManager.instance.PlayRandomSFXClip(activeFootsteps, transform, volume);
    }

    public void setFootstepType(footstepTypes footstepType)
    {
        switch (footstepType)
        {
            case footstepTypes.Grass:
                activeFootsteps = grassFootsteps;
                break;
            case footstepTypes.Brick:
                activeFootsteps = brickFootsteps;
                break;
            case footstepTypes.Prop:
                activeFootsteps = propFootsteps;
                break;
            case footstepTypes.Untagged:
                activeFootsteps = brickFootsteps;
                break;
        }
    }
}
