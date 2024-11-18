using UnityEngine;
using UnityEngine.Rendering;

public class SFXManager : MonoBehaviour
{

    public static SFXManager instance;

    [SerializeField] private AudioSource soundFXObject;

    [Header("Music")]
    [SerializeField] private bool playMusic;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip musicClip;

    private void Awake()
    {
        if(instance == null) {
            instance = this;
        }
    }

    private void Start()
    {
        if (playMusic)
            PlayMusic();
    }

    private void PlayMusic()
    {
        //spawn in GameObject
        AudioSource audioSrc = Instantiate(musicSource, Vector3.zero, Quaternion.identity);

        // parent to camera
        audioSrc.transform.parent = FindFirstObjectByType<Camera>().transform;
        audioSrc.transform.localPosition = Vector3.zero;

        //assign properties and play
        audioSrc.clip = musicClip;
        audioSrc.Play();

        //loop
        audioSrc.loop = true;
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
