using System;
using UnityEngine;

public class PlayerSFXManager : MonoBehaviour
{
    private PlayerMovement pm;
    private Rigidbody rb;
    private FootstepAudio stepAudio;

    public AudioClip jump;
    public AudioClip dash;

    [Header("Slide")]
    public AudioSource slideSounds;
    public float minSlideVolume;
    public float maxSlideVolume;
    public float maxSlideSpeed;

    [Header("High Speed")]
    public AudioSource windSounds;
    public float minWindVolume;
    public float maxWindVolume;
    public float requiredSpeed;
    public float maxWindSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        stepAudio = GetComponentInChildren<FootstepAudio>();
    }

    void Update()
    {
        if (pm.grounded)
        {
            FootstepAudio.footstepTypes ground;
            Enum.TryParse(groundType(), out ground);

            stepAudio.setFootstepType(ground);
        }
    }

    private string groundType()
    {
        RaycastHit hit;
        Physics.Raycast(rb.position, Vector3.down, out hit, pm.playerHeight * 0.5f + 0.3f, pm.whatIsGround);

        if (hit.collider != null)
        {
            return hit.collider.tag;
        }
        else
            return null;
    }

    public void PlayJumpSfx()
    {
        SFXManager.instance.PlaySFXClip(jump, pm.transform, 1);
    }

    public void PlayDashSfx()
    {
        SFXManager.instance.PlaySFXClip(dash, pm.transform, 1);
    }

    public void PlaySlideSfx(float speed)
    {
        float volume = 0;
        if(speed > 0 && speed < maxSlideSpeed)
        {
            volume = Mathf.Lerp(minSlideVolume, maxSlideVolume, speed / maxSlideSpeed);
        } else if( speed > maxSlideSpeed)
        {
            volume = maxSlideVolume;
        }

        if (!slideSounds.isPlaying)
        {
            slideSounds.Play();
        }

        slideSounds.volume = volume;
    }

    public void StopSlideSfx()
    {
        if (slideSounds.isPlaying)
        {
            slideSounds.Stop();
        }
    }

    public void TryWindSfx(float speed)
    {
        if(speed >= requiredSpeed)
        {
            PlayWindSfx(speed);
        } else
        {
            StopWindSfx();
        }
    }

    public void PlayWindSfx(float speed)
    {
        float volume = 0;
        if (speed > requiredSpeed && speed < maxWindSpeed)
        {
            volume = Mathf.Lerp(minWindVolume, maxWindVolume, speed / maxWindSpeed);
        }
        else if (speed > maxWindSpeed)
        {
            volume = maxWindVolume;
        }

        if (!windSounds.isPlaying)
        {
            windSounds.Play();
        }

        windSounds.volume = volume;
    }

    public void StopWindSfx()
    {
        if (windSounds.isPlaying)
        {
            windSounds.Stop();
        }
    }

}
