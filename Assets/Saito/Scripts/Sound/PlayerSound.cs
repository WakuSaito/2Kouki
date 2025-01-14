using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private SoundManager mSoundManager;
    private AudioSource mAudioSource;

    private void Awake()
    {
        mSoundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        mAudioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlayFootStep()
    {
        AudioClip random_se = mSoundManager.playerFootSteps[Random.Range(0, mSoundManager.playerFootSteps.Length)];
        mAudioSource.PlayOneShot(random_se);
    }
    public void PlayPickUp()
    {
        mAudioSource.PlayOneShot(mSoundManager.playerPickUp);
    }
    public void PlayEat()
    {
        mAudioSource.PlayOneShot(mSoundManager.playerEat);
    }
    public void PlayDrink()
    {
        mAudioSource.PlayOneShot(mSoundManager.playerDrink);
    }
    public void PlayHeal()
    {
        mAudioSource.PlayOneShot(mSoundManager.playerHeal);
    }
    public void PlayDamage()
    {
        mAudioSource.PlayOneShot(mSoundManager.playerDamage);
    }
    public void PlayWhistleAttack()
    {
        mAudioSource.PlayOneShot(mSoundManager.whistleAttack);
    }
    public void PlayWhistleDetect()
    {
        mAudioSource.PlayOneShot(mSoundManager.whistleDetect);
    }
}
