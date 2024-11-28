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
        audioSource.PlayOneShot(soundManager.zombieFootStep);
    }
    public void PlayVoice()
    {
        audioSource.PlayOneShot(soundManager.zombieVoice);
    }
    public void PlayDamage()
    {
        audioSource.PlayOneShot(soundManager.zomvieDamage);
    }
    public void PlayDead()
    {
        audioSource.PlayOneShot(soundManager.zomvieDead);
    }

}
