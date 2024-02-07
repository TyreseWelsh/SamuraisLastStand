using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainSounds : MonoBehaviour
{
    [SerializeField] AudioSource mainSrc, swordSrc;
    [SerializeField] AudioClip footstep1, footstep2, footstep3, footstep4;
    [SerializeField] AudioClip swordSwing1, swordSwing2, hit;
    [SerializeField] AudioClip death;

    public void PlayFootstepSound()
    {
        int randNum = Random.Range(1, 5);
        mainSrc.reverbZoneMix = 1;
        mainSrc.pitch = 1;
        mainSrc.volume = 0.05f;
        switch(randNum)
        {
            case 1:
                mainSrc.PlayOneShot(footstep1);
                break;
            case 2:
                mainSrc.PlayOneShot(footstep2);
                break;
            case 3:
                mainSrc.PlayOneShot(footstep3);
                break;
            case 4:
                mainSrc.PlayOneShot(footstep4);
                break;
        }
    }

    public void PlaySwordSwingSound()
    {
        int randNum = Random.Range(1, 3);
        swordSrc.volume = 0.8f;
        switch (randNum)
        {
            case 1:
                swordSrc.PlayOneShot(swordSwing1);
                break;
            case 2:
                swordSrc.PlayOneShot(swordSwing2);
                break;
        }
    }

    public void PlayHitSound()
    {
        swordSrc.pitch = 0.85f;
        swordSrc.PlayOneShot(hit);
    }

    public void PlayDeathSound()
    {
        mainSrc.reverbZoneMix = 1.1f;
        mainSrc.volume = 0.4f;
        mainSrc.pitch = 0.9f;
        mainSrc.PlayOneShot(death);
    }
}
