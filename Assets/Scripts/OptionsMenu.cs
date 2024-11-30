using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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
