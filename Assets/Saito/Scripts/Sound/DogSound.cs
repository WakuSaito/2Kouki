using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogSound : DogBase
{
    private SoundManager soundManager;
    private AudioSource audioSource;

    public override void SetUpDog()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlayFootStep()
    {
        audioSource.PlayOneShot(soundManager.dogFootStep);
    }
    public void PlayAttackBark()
    {
        audioSource.PlayOneShot(soundManager.dogAttackBark);
    }
    public void PlayDetectBark()
    {
        audioSource.PlayOneShot(soundManager.dogDetectBark);
    }
}
