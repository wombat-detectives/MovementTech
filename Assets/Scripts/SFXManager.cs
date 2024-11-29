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
    private Camera mCam;
    private Transform musicPlayer;

    private void Awake()
    {
        if(instance == null) {
            instance = this;
        }
    }

    private void Start()
    {
        if (playMusic)
        {
            mCam = Camera.main;
            PlayMusic();
            if (musicPlayer != null)
                musicPlayer.gameObject.SetActive(true);
        }
        else
        {
            if (musicPlayer != null)
                musicPlayer.gameObject.SetActive(false);
        }
            
    }

    private void Update()
    {
        if (playMusic && musicPlayer != null)
        {
            // parent to camera
            musicPlayer.transform.position = mCam.transform.position;
        }
    }

    private void PlayMusic()
    {
        //Find existing music player
        GameObject[] players = GameObject.FindGameObjectsWithTag("MusicPlayer");

        AudioSource audioSrc = null;
        if (players.Length > 0)
        {
            audioSrc = players[0].GetComponent<AudioSource>();
        }
        else
        {
            //spawn in GameObject
            audioSrc = Instantiate(musicSource, Vector3.zero, Quaternion.identity);
        }

        musicPlayer = audioSrc.transform;

        //assign properties and play
        if(audioSrc.resource != musicClip)
        {
            audioSrc.Stop();
            audioSrc.clip = musicClip;
            audioSrc.Play();
        }

        //loop
        audioSrc.loop = true;
    }

    public void PlaySFXClip(AudioClip clip, Transform spawnTransform, float volume)
    {
        //spawn in GameObject
        AudioSource audioSrc = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        //parent to transform
        audioSrc.transform.parent = spawnTransform;

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

        //parent to transform
        audioSrc.transform.parent = spawnTransform;

        //assign properties and play
        audioSrc.clip = clips[rand];
        audioSrc.volume = volume;
        audioSrc.Play();

        //get length of clip and destroy
        float clipLength = audioSrc.clip.length;
        Destroy(audioSrc.gameObject, clipLength);
    }
}
