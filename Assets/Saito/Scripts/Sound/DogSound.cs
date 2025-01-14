using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogSound : DogBase
{
    private SoundManager mSoundManager;
    private AudioSource mAudioSource;

    public override void SetUpDog()
    {
        mSoundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        mAudioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlayFootStep()
    {
        mAudioSource.PlayOneShot(mSoundManager.dogFootStep);
    }
    public void PlayAttackBark()
    {
        mAudioSource.PlayOneShot(mSoundManager.dogAttackBark);
    }
    public void PlayDetectBark()
    {
        mAudioSource.PlayOneShot(mSoundManager.dogDetectBark);
    }
}
