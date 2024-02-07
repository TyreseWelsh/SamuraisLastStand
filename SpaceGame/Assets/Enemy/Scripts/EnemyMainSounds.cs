using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMainSounds : MonoBehaviour
{
    [SerializeField] AudioSource mainSrc;
    [SerializeField] AudioClip footstep1, footstep2, footstep3, footstep4;
    [SerializeField] AudioClip attack;


    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayFootstepSound()
    {
        int randNum = Random.Range(1, 5);

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
            default:
                mainSrc.PlayOneShot(footstep1);
                break;
        }
    }

    public void PlayAttackSound()
    {
        mainSrc.PlayOneShot(attack);
    }
}
