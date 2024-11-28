using UnityEngine;

public class LevelTransitionSFX : MonoBehaviour
{
    public AudioClip inSound;
    public AudioClip outSound;

    public void PlayOutSound()
    {
        SFXManager.instance.PlaySFXClip(outSound, Camera.main.transform, 1);
    }

    public void PlayInSound()
    {
        SFXManager.instance.PlaySFXClip(inSound, Camera.main.transform, 1);
    }
}
