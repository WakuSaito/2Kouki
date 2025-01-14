using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSound : ZombieBase
{
    private SoundManager mSoundManager;
    private AudioSource mAudioSource;

    public override void SetUpZombie()
    {
        mSoundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        mAudioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlayFootStep()
    {
        AudioClip random_se = mSoundManager.zombieFootStep[Random.Range(0, mSoundManager.zombieFootStep.Length)];
        mAudioSource.PlayOneShot(random_se);
    }
    public void PlayVoice()
    {
        mAudioSource.PlayOneShot(mSoundManager.zombieVoice);
    }
    public void PlayDamage()
    {
        mAudioSource.PlayOneShot(mSoundManager.zombieDamage);
    }
    public void PlayDead()
    {
        mAudioSource.PlayOneShot(mSoundManager.zombieDead);
    }

}
