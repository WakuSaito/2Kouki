using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSound : ZombieBase
{
    private SoundManager soundManager;
    private AudioSource audioSource;

    public override void SetUpZombie()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlayFootStep()
    {
        AudioClip randomSE = soundManager.zombieFootStep[Random.Range(0, soundManager.zombieFootStep.Length)];
        audioSource.PlayOneShot(randomSE);
    }
    public void PlayVoice()
    {
        audioSource.PlayOneShot(soundManager.zombieVoice);
    }
    public void PlayDamage()
    {
        audioSource.PlayOneShot(soundManager.zombieDamage);
    }
    public void PlayDead()
    {
        audioSource.PlayOneShot(soundManager.zombieDead);
    }

}
