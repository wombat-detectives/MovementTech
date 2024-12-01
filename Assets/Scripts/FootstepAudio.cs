
using UnityEngine;

public class FootstepAudio : MonoBehaviour
{

    public float volume;

    [Header("Footsteps")]
    public AudioClip[] grassFootsteps;
    public AudioClip[] brickFootsteps;
    public AudioClip[] propFootsteps;

    public enum footstepTypes
    {
        Untagged,
        Grass,
        Brick,
        Prop
    }

    private AudioClip[] activeFootsteps;

    public void playFootstep()
    {
        if (SFXManager.instance == null)
        {
            Debug.LogError("SFXManager instance is null! Make sure SFXManager is in the scene.");
            return;
        }

        if (activeFootsteps == null || activeFootsteps.Length == 0)
        {
            Debug.LogWarning("No active footsteps assigned or the array is empty!");
            return;
        }

        SFXManager.instance.PlayRandomSFXClip(activeFootsteps, transform, volume);
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
