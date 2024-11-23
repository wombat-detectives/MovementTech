using System.Collections;
using UnityEngine;

public class Radio : MonoBehaviour
{
    private float oldVolume = 1f;
    private AudioSource music;

    public float fadeTime = 0.5f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MusicPlayer"))
        {
            if(music == null)
            {
                music = other.gameObject.GetComponent<AudioSource>();
            }
            StopAllCoroutines();
            StartCoroutine(fadeMusicOut());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("MusicPlayer"))
        {
            if (music == null)
                music = other.gameObject.GetComponent<AudioSource>();
            StopAllCoroutines();
            StartCoroutine(fadeMusicIn());
        }
    }

    private IEnumerator fadeMusicOut()
    {
        float elapsedTime = 0f;

        float initialVol = music.volume;

        while(elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;

            music.volume = Mathf.Lerp(initialVol, 0f, elapsedTime / fadeTime);
            yield return null;
        }
    }

    private IEnumerator fadeMusicIn()
    {
        float elapsedTime = 0f;

        float initialVol = music.volume;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;

            music.volume = Mathf.Lerp(initialVol, oldVolume, elapsedTime / fadeTime);
            yield return null;
        }
    }
}
