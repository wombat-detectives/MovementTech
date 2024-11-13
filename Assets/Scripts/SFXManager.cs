using UnityEngine;

public class SFXManager : MonoBehaviour
{

    public static SFXManager instance;

    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if(instance == null) {
            instance = this;
        }
    }

    public void PlaySFXClip(AudioClip clip, Transform spawnTransform, float volume)
    {
        //spawn in GameObject
        AudioSource audioSrc = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        //assign properties and play
        audioSrc.clip = clip;
        audioSrc.volume = volume;
        audioSrc.Play();

        //get length of clip and destroy
        float clipLength = audioSrc.clip.length;
        Destroy(audioSrc.gameObject, clipLength);
    }

    public void PlayRandomSFXClip(AudioClip[] clips, Transform spawnTransform, float volume)
    {
        //randomizer
        int rand = Random.Range(0, clips.Length);

        //spawn in GameObject
        AudioSource audioSrc = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        //assign properties and play
        audioSrc.clip = clips[rand];
        audioSrc.volume = volume;
        audioSrc.Play();

        //get length of clip and destroy
        float clipLength = audioSrc.clip.length;
        Destroy(audioSrc.gameObject, clipLength);
    }
}
