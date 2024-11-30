using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public AudioClip[] sfxSounds;

    public float sfxTimeInterval = 1f;
    private float timer = 0f;

    private void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
    }

    private void Awake()
    {
        Slider[] sliders = GetComponentsInChildren<Slider>();

        foreach(Slider sl in sliders) {
            Debug.Log(sl.name);
            float currVol;
            switch (sl.name)
            {
                case "MainVolumeSlider":
                    audioMixer.GetFloat("masterVolume", out currVol);
                    sl.value = Mathf.Pow(10f, currVol / 20);
                    break;
                case "SFXVolumeSlider":
                    audioMixer.GetFloat("sfxVolume", out currVol);
                    sl.value = Mathf.Pow(10f, currVol / 20);
                    break;
                case "MusicVolumeSlider":
                    audioMixer.GetFloat("musicVolume", out currVol);
                    sl.value = Mathf.Pow(10f, currVol / 20);
                    break;

            }
        }
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(volume) * 20f);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20f);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(volume) * 20f);
        if (sfxSounds.Length > 0 && timer <= 0)
        {
            SFXManager.instance.PlayRandomSFXClip(sfxSounds, Camera.main.transform, 1);
            timer = sfxTimeInterval;
        }
            
    }
}
